#!/bin/bash

while true
do
tmwserv-game --v 3
echo "server crashed on `date`" > tmwserv-game_last_crash.txt
sleep 5
done