//
//  Connection.cs
//
//  This file is part of Invertika (http://invertika.org)
// 
//  Based on The Mana Server (http://manasource.org)
//  Copyright (C) 2004-2012  The Mana World Development Team 
//
//  Author:
//       seeseekey <seeseekey@googlemail.com>
// 
//  Copyright (c) 2011, 2012 by Invertika Development Team
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISL.Server.Network;
using System.Net.Sockets;
using ISL.Server;
using ISL.Server.Utilities;

namespace invertika_game.Network
{
    //TODO Connection zu Netcomputer zusammenfassen? - bzw das ganze für Connection und COnnectionHandler
    public class Connection
    {
        TcpClient mRemote;
        //ENetPeer* mRemote;
        //ENetHost* mLocal;

        public Connection()
        {
            //mRemote(0),
            //mLocal(0)
        }

        public bool start(string address, int port)
        {
            try
            {
                mRemote=new TcpClient(address, port);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void stop()
        {
            if(mRemote!=null)
            {
                mRemote.Close();
            }

            mRemote=null;
            //mLocal = 0;
        }

        public bool isConnected()
        {
            return mRemote.Connected;
        }

        public void send(MessageOut msg)//, bool reliable, uint channel)
        {
            NetworkStream stream=mRemote.GetStream();
       
            //In Websocketpaket packen
            byte[] wsMsg=Websocket.GetWebsocketDataFrame(msg.getData());
            stream.Write(wsMsg); 

            if(mRemote==null)
            {
                Logger.Write(LogLevel.Warning, "Can't send message to unconnected host! ({0})", msg);
                return;
            }

            Program.gBandwidth.increaseInterServerOutput(msg.getLength());
        }

        public void process()
        {
            //ENetEvent event;
            //// Process Enet events and do not block.
            //while (enet_host_service(mLocal, &event, 0) > 0)
            //{
            //    switch (event.type)
            //    {
            //        case ENET_EVENT_TYPE_RECEIVE:
            //            if (event.packet.dataLength >= 2)
            //            {
            //                MessageIn msg((char *)event.packet.data,
            //                              event.packet.dataLength);
            //                gBandwidth.increaseInterServerInput(event.packet.dataLength);
            //                processMessage(msg);
            //            }
            //            else
            //            {
            //                LOG_WARN("Message too short.");
            //            }
            //            // Clean up the packet now that we are done using it.
            //            enet_packet_destroy(event.packet);
            //            break;

            //        default:
            //            break;
            //    }
        }
    }
}
