#!/bin/bash

# mail options
SUBJECT="Crashmeldung - manaserv-account"
EMAILTO="webmaster@invertika.org"

while true
do
  rm core.*
  ulimit -c unlimited
  manaserv-account --v 3
  FILE=ls core.*
  echo "Letzter Servercrash am `date`" > last_crash_manaserv-account-txt
  /bin/mail -s "$SUBJECT" "$EMAILTO" < $FILE
  sleep 5
done