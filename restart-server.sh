#!/bin/bash

pkill -SIGTERM tmwserv
sleep 1
./start-tmwserv-account.sh &
sleep 5
./start-tmwserv-game.sh &