/*
 *  The Mana Server
 *  Copyright (C) 2004-2010  The Mana World Development Team
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

#ifndef RESOURCE_MANAGER_H
#define RESOURCE_MANAGER_H

#include <string>

namespace ResourceManager
{
    /**
     * Searches for zip files and adds them to PhysFS search path.
     */
    void initialize();

    /**
     * Checks whether the given file or directory exists in the search path
     */
    bool exists(const std::string &path);

    /**
     * Allocates data into a buffer pointer for raw data loading. The
     * returned data is expected to be freed using <code>free()</code>.
     *
     * @param fileName The name of the file to be loaded.
     * @param fileSize The size of the file that was loaded.
     *
     * @return An allocated byte array containing the data that was loaded,
     *         or <code>NULL</code> on failure.
     * @note The array contains an extra \0 character at position fileSize.
     */
    char *loadFile(const std::string &fileName, int &fileSize,
                   bool removeBOM = false);
}

#endif
