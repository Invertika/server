#!/bin/bash

./stop-tmwserv-account.sh &
sleep 1
./stop-tmwserv-game.sh &
sleep 1
./start-tmwserv-account.sh &
sleep 5
./start-tmwserv-game.sh &
