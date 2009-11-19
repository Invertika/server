#!/bin/bash

pkill -SIGTERM manaserv
sleep 1
./start-manaserv-account.sh &
sleep 5
./start-manaserv-game.sh &