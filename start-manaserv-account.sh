#!/bin/bash

while true
do
manaserv-account --v 3
echo "server crashed on `date`" > manaserv-account_last_crash.txt
sleep 5
done