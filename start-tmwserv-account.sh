#!/bin/bash

while true
do
tmwserv-account --v 3
echo "server crashed on `date`" > tmwserv-account_last_crash.txt
sleep 5
done