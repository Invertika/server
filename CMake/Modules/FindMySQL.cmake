# - Find MySQL
# Find the MySQL includes and client library
# This module defines
#  MYSQL_INCLUDE_DIR, where to find mysql.h
#  MYSQL_LIBRARIES, the libraries needed to use MySQL.
#  MYSQL_FOUND, If false, do not try to use MySQL.

IF(MYSQL_INCLUDE_DIR AND MYSQL_LIBRARIES)
   SET(MYSQL_FOUND TRUE)

ELSE(MYSQL_INCLUDE_DIR AND MYSQL_LIBRARIES)

  FIND_PATH(MYSQL_INCLUDE_DIR mysql.h
      /usr/include/mysql
      /usr/local/include/mysql
      $ENV{ProgramFiles}/MySQL/*/include
      $ENV{SystemDrive}/MySQL/*/include
      )

  IF(WIN32 AND MSVC)
    FIND_LIBRARY(MYSQL_LIBRARIES NAMES libmysql
        PATHS
        $ENV{ProgramFiles}/MySQL/*/lib/opt
        $ENV{SystemDrive}/MySQL/*/lib/opt
        )
  ELSE(WIN32 AND MSVC)
    FIND_LIBRARY(MYSQL_LIBRARIES NAMES mysqlclient
        PATHS
        /usr/lib/mysql
        /usr/local/lib/mysql
        )
  ENDIF(WIN32 AND MSVC)

  IF(MYSQL_INCLUDE_DIR AND MYSQL_LIBRARIES)
      SET(MYSQL_FOUND TRUE)
      MESSAGE(STATUS "Found MySQL: ${MYSQL_INCLUDE_DIR}, ${MYSQL_LIBRARIES}")
  ELSE(MYSQL_INCLUDE_DIR AND MYSQL_LIBRARIES)
      SET(MYSQL_FOUND FALSE)
      MESSAGE(STATUS "MySQL not found.")
  ENDIF(MYSQL_INCLUDE_DIR AND MYSQL_LIBRARIES)

  MARK_AS_ADVANCED(MYSQL_INCLUDE_DIR MYSQL_LIBRARIES)

ENDIF(MYSQL_INCLUDE_DIR AND MYSQL_LIBRARIES)
