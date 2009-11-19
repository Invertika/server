#!/bin/bash

while true
do
manaserv-game --v 3
echo "server crashed on `date`" > manaserv-game_last_crash.txt
sleep 5
done