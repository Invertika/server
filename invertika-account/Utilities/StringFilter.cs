﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_account.Utilities
{
	public class StringFilter
	{
		//        typedef std::list<std::string> Slangs;
		//typedef Slangs::iterator SlangIterator;
		//Slangs mSlangs;    /**< the formatted Slangs list */
		bool mInitialized;                 /**< Set if the list is loaded */

		public StringFilter()
		{
			mInitialized=false;
			//loadSlangFilterList();
		}

		~StringFilter()
		{
			//writeSlangFilterList();
		}

		bool loadSlangFilterList()
		{
			mInitialized=false;

			//const std::string slangsList = Configuration::getValue("SlangsList",
			//                                                       std::string());
			//if (!slangsList.empty()) {
			//    std::istringstream iss(slangsList);
			//    std::string tmp;
			//    while (getline(iss, tmp, ','))
			//        mSlangs.push_back(tmp);
			//    mInitialized = true;
			//}

			return mInitialized;
		}

		void writeSlangFilterList()
		{
			//// Write the list to config
			//std::string slangsList;
			//for (SlangIterator i = mSlangs.begin(); i != mSlangs.end(); )
			//{
			//    slangsList += *i;
			//    ++i;
			//    if (i != mSlangs.end()) slangsList += ",";
			//}
		}

		public bool filterContent(string text)
		{
			//if (!mInitialized) {
			//    LOG_DEBUG("Slangs List is not initialized.");
			//    return true;
			//}

			bool isContentClean=true;
			//std::string upperCaseText = text;

			//std::transform(text.begin(), text.end(), upperCaseText.begin(),
			//        (int(*)(int))std::toupper);

			//for (Slangs::const_iterator i = mSlangs.begin(); i != mSlangs.end(); ++i)
			//{
			//    // We look for slangs into the sentence.
			//    std::string upperCaseSlang = *i;
			//    std::transform(upperCaseSlang.begin(), upperCaseSlang.end(),
			//            upperCaseSlang.begin(), (int(*)(int))std::toupper);

			//    if (upperCaseText.compare(upperCaseSlang)) {
			//        isContentClean = false;
			//        break;
			//    }
			//}

			return isContentClean;
		}

		bool isEmailValid(string email)
		{
			//unsigned int min = Configuration::getValue("account_minEmailLength", 7);
			//unsigned int max = Configuration::getValue("account_maxEmailLength", 128);

			//// Testing email validity
			//if (email.length() < min || email.length() > max)
			//{
			//    return false;
			//}

			//std::string::size_type atpos = email.find_first_of('@');

			//// TODO Find some nice regex for this...
			//return (atpos != std::string::npos) &&
			//    (email.find_first_of('.', atpos) != std::string::npos) &&
			//    (email.find_first_of(' ') == std::string::npos);

			return true; //ssk
		}

		public bool findDoubleQuotes(string text)
		{
			//return (text.find('"', 0) != std::string::npos);
			return false; //ssk
		}
	}
}