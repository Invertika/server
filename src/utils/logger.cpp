/*
 *  The Mana Server
 *  Copyright (C) 2004-2010  The Mana World Development Team
 *  Copyright (C) 2010  The Mana Development Team
 *
 *  This file is part of The Mana Server.
 *
 *  The Mana Server is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  any later version.
 *
 *  The Mana Server is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with The Mana Server.  If not, see <http://www.gnu.org/licenses/>.
 */

#include "logger.h"
#include "common/resourcemanager.hpp"
#include "utils/string.hpp"

#include <ctime>
#include <fstream>
#include <iomanip>
#include <iostream>

#ifdef WIN32
#include <windows.h>
#endif

namespace utils
{
/** Log file. */
static std::ofstream mLogFile;
/** current log filename */
std::string Logger::mFilename = "";
/** Timestamp flag. */
bool Logger::mHasTimestamp = true;
/** Tee mode flag. */
bool Logger::mTeeMode = false;
/** Verbosity level. */
Logger::Level Logger::mVerbosity = Logger::Info;
/** Enables logrotation by size of the logfile. */
bool Logger::mLogRotation = false;
/** Maximum size of current logfile. */
long Logger::mMaxFileSize = 1024; // 1 Mb
/** Switch log file each day. */
bool Logger::mSwitchLogEachDay = false;
/** Last call date */
static std::string mLastCallDate = "";

/**
  * Gets the current time.
  *
  * @return the current time as string.
  */
static std::string getCurrentTime()
{
    time_t now;
    tm local;

    // Get current time_t value
    time(&now);

    // Convert time_t to tm struct to break the time into individual
    // constituents.
    local = *(localtime(&now));

    // Stringify the time, the format is: hh:mm:ss
    using namespace std;
    ostringstream os;
    os << setw(2) << setfill('0') << local.tm_hour
       << ":" << setw(2) << setfill('0') << local.tm_min
       << ":" << setw(2) << setfill('0') << local.tm_sec;

    return os.str();
}

/**
  * Gets the current date.
  *
  * @return the current date as string.
  */
static std::string getCurrentDate()
{
    time_t now;
    tm local;

    // Get current time_t value
    time(&now);

    // Convert time_t to tm struct to break the time into individual
    // constituents.
    local = *(localtime(&now));

    // Stringify the time, the format is: yyyy-mm-dd
    using namespace std;
    ostringstream os;
    os << setw(4) << setfill('0') << (local.tm_year + 1900)
       << "-" << setw(2) << setfill('0') << local.tm_mon
       << "-" << setw(2) << setfill('0') << local.tm_mday;

    return os.str();
}

/**
  * Check whether the day has changed since the last call.
  *
  * @return whether the day has changed.
  */
bool getDayChanged()
{
    static std::string date = getCurrentDate();
    if (mLastCallDate != date)
    {
        // Reset the current date for next call.
        mLastCallDate = date;
        return true;
    }
    return false;
}

void Logger::output(std::ostream &os, const std::string &msg, const char *prefix)
{
    if (mHasTimestamp)
    {
        os << "[" << getCurrentTime() << "]" << ' ';
    }

    if (prefix)
    {
        os << prefix << ' ';
    }

    os << msg << std::endl;
}

void Logger::setLogFile(const std::string &logFile, bool append)
{
    // Close the current log file.
    if (mLogFile.is_open())
    {
        mLogFile.close();
    }

    // Open the file for output
    // and remove the former file contents depending on the append flag.
    mLogFile.open(logFile.c_str(),
                  append ? std::ios::app : std::ios::trunc);

    mFilename = logFile;
    mLastCallDate = getCurrentDate();

    if (!mLogFile.is_open())
    {
        throw std::ios::failure("unable to open " + logFile + "for writing");
    }
    else
    {
        // By default the streams do not throw any exception
        // let std::ios::failbit and std::ios::badbit throw exceptions.
        mLogFile.exceptions(std::ios::failbit | std::ios::badbit);
    }
}

void Logger::output(const std::string &msg, Level atVerbosity)
{
    static const char *prefixes[] =
    {
#ifdef T_COL_LOG
        "[\033[45mFTL\033[0m]",
        "[\033[41mERR\033[0m]",
        "[\033[43mWRN\033[0m]",
#else
        "[FTL]",
        "[ERR]",
        "[WRN]",
#endif
        "[INF]",
        "[DBG]"
    };

    if (mVerbosity >= atVerbosity)
    {
        bool open = mLogFile.is_open();

        if (open)
        {
            output(mLogFile, msg, prefixes[atVerbosity]);
            switchLogs();
        }

        if (!open || mTeeMode)
        {
            output(atVerbosity <= Warn ? std::cerr : std::cout,
                   msg, prefixes[atVerbosity]);
        }
    }
}

void Logger::switchLogs()
{
    // Handles logswitch if enabled
    // and if at least one switch condition permits it.
    if (!mLogRotation || (mMaxFileSize <= 0 && !mSwitchLogEachDay))
        return;

    // Update current filesize
    long fileSize = mLogFile.tellp();

    if ((fileSize >= (mMaxFileSize * 1024))
        || (mSwitchLogEachDay && getDayChanged()))
    {
        // Close logfile, rename it and open a new one
        mLogFile.flush();
        mLogFile.close();

        // Stringify the time, the format is: path/yyyy-mm-dd-n_logFilename.
        using namespace std;
        ostringstream os;
        os << getCurrentDate();

        int fileNum = 1;
        ResourceManager::splittedPath filePath =
                               ResourceManager::splitFileNameAndPath(mFilename);

        std::string newFileName;
        // Keeping a hard limit of 100 files per day.
        do
        {
            newFileName = filePath.path + os.str()
                                + "-" + toString<int>(fileNum)
                                + "_" + filePath.file;
        }
        while (ResourceManager::exists(newFileName, false) && ++fileNum < 100);

        if (rename(mFilename.c_str(), newFileName.c_str()) != 0)
        {
            // Continue appending on the original file.
            setLogFile(mFilename, true);
            mLogFile << "Error renaming file: " << mFilename << " to: "
            << newFileName << std::endl << "Keep logging on the same log file."
            << std::endl;
        }
        else
        {
            // Keep the logging after emptying the original log file.
            setLogFile(mFilename);
            mLogFile << "---- Continue logging from former file " << newFileName
                     << " ----" << std::endl;
        }
    }
}

} // namespace utils
