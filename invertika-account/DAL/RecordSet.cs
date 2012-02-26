using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_account.DAL
{
 //   public class RecordSet
 //   {
 //       /**
 //* Data type for a row in a RecordSet.
 //*/
 //       //typedef std::vector<std::string> Row;

 //       List<string> mHeaders; /**< a list of field names */
 //       List<List<string>> mRows;

 //       public RecordSet()
 //       {
 //           mHeaders=new List<string>();
 //           mRows=new List<List<string>>();
 //       }

 //       ~RecordSet()
 //       {
 //       }

 //       /**
 //        * Remove all the Records.
 //        */
 //       public void clear()
 //       {
 //           mHeaders.Clear();
 //           mRows.Clear();
 //       }

 //       /**
 //        * Check if the RecordSet is empty.
 //        */
 //       public bool isEmpty()
 //       {
 //           return mRows.Count==0;
 //       }

 //       /**
 //        * Get the number of rows.
 //        *
 //        * @return the number of rows.
 //        */
 //       uint rows()
 //       {
 //           return (uint)mRows.Count;
 //       }

 //       /**
 //        * Get the number of columns.
 //        *
 //        * @return the number of columns.
 //        */
 //       uint cols()
 //       {
 //           return (uint)mHeaders.Count;
 //       }

 //       /**
 //        * Set the column headers.
 //        */
 //       public void setColumnHeaders(List<string> headers)
 //       {
 //           if(mHeaders.Count>0)
 //           {
 //               throw new AlreadySetException();
 //           }

 //           mHeaders=headers;
 //       }

 //       /**
 //        * Add a new row.
 //        */
 //       public void add(List<string> row)
 //       {
 //           uint nCols = (uint)mHeaders.Count();

 //           if (nCols == 0) 
 //           {
 //               throw new RsColumnHeadersNotSetException();
 //           }

 //           if (row.Count() != nCols) 
 //           {
 //               //std::ostringstream msg;
 //               //msg << "row has " << row.size() << " columns; "
 //               //    << "expected: " << nCols << std::ends;

 //               //throw std::invalid_argument(msg.str());

 //               throw new Exception();
 //           }

 //           mRows.Add(row);
 //       }

 //       //string operator()(const unsigned int row, const unsigned int col) const
 //       //{
 //       //    if ((row >= mRows.size()) || (col >= mHeaders.size())) {
 //       //        std::ostringstream os;
 //       //        os << "(" << row << ", " << col << ") is out of range; "
 //       //           << "max rows: " << mRows.size()
 //       //           << ", max cols: " << mHeaders.size() << std::ends;

 //       //        throw std::out_of_range(os.str());
 //       //    }

 //       //    return mRows[row][col];
 //       //}

 //       //const std::string &RecordSet::operator()(const unsigned int row,
 //       //                                         const std::string& name) const
 //       //{
 //       //    if (row >= mRows.size()) {
 //       //        std::ostringstream os;
 //       //        os << "row " << row << " is out of range; "
 //       //           << "max rows: " << mRows.size() << std::ends;

 //       //        throw std::out_of_range(os.str());
 //       //    }

 //       //    Row::const_iterator it = std::find(mHeaders.begin(),
 //       //                                       mHeaders.end(),
 //       //                                       name);
 //       //    if (it == mHeaders.end()) {
 //       //        std::ostringstream os;
 //       //        os << "field " << name << " does not exist." << std::ends;

 //       //        throw std::invalid_argument(os.str());
 //       //    }

 //       //    // find the field index.
 //       //    const unsigned int nCols = mHeaders.size();
 //       //    unsigned int i;
 //       //    for (i = 0; i < nCols; ++i) {
 //       //        if (mHeaders[i] == name) {
 //       //            break;
 //       //        }
 //       //    }

 //       //    return mRows[row][i];
 //       //}

 //       //std::ostream &operator<<(std::ostream &out, const RecordSet &rhs)
 //       //{
 //       //    // print the field names first.
 //       //    if (rhs.mHeaders.size() > 0) {
 //       //        out << "|";
 //       //        for (Row::const_iterator it = rhs.mHeaders.begin();
 //       //             it != rhs.mHeaders.end();
 //       //             ++it)
 //       //        {
 //       //            out << (*it) << "|";
 //       //        }
 //       //        out << std::endl << std::endl;
 //       //    }

 //       //    // and then print every line.
 //       //    for (RecordSet::Rows::const_iterator it = rhs.mRows.begin();
 //       //         it != rhs.mRows.end();
 //       //         ++it)
 //       //    {
 //       //        out << "|";
 //       //        for (Row::const_iterator it2 = (*it).begin();
 //       //             it2 != (*it).end();
 //       //             ++it2)
 //       //        {
 //       //            out << (*it2) << "|";
 //       //        }
 //       //        out << std::endl;
 //       //    }

 //       //    return out;
 //       //}
 //   }
}