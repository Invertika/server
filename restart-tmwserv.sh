#!/bin/bash

./stop-tmwserv.sh
sleep 1
./start-tmwserv-account.sh &
sleep 5
./start-tmwserv-game.sh &