using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISL.Server.Network;
using System.Net.Sockets;

namespace invertika_game.Network
{
	public class Connection
	{
		TcpClient mRemote;
		//ENetPeer* mRemote;
		//ENetHost* mLocal;

		//#ifdef ENET_VERSION_CREATE
		//#define ENET_CUTOFF ENET_VERSION_CREATE(1,3,0)
		//#else
		//#define ENET_CUTOFF 0xFFFFFFFF
		//#endif

		public Connection()
		{
			//mRemote(0),
			//mLocal(0)
		}

		public bool start(string address, int port)
		{
			mRemote=new TcpClient(address, port);

			//    ENetAddress enetAddress;
			//    enet_address_set_host(&enetAddress, address.c_str());
			//    enetAddress.port = port;

			//#if defined(ENET_VERSION) && ENET_VERSION >= ENET_CUTOFF
			//    mLocal = enet_host_create(NULL /* create a client host */,
			//                              1 /* allow one outgoing connection */,
			//                              0           /* unlimited channel count */,
			//                              0 /* assume any amount of incoming bandwidth */,
			//                              0 /* assume any amount of outgoing bandwidth */);
			//#else
			//    mLocal = enet_host_create(NULL /* create a client host */,
			//                              1 /* allow one outgoing connection */,
			//                              0 /* assume any amount of incoming bandwidth */,
			//                              0 /* assume any amount of outgoing bandwidth */);
			//#endif

			//    if (!mLocal)
			//        return false;

			//    // Initiate the connection, allocating channel 0.
			//#if defined(ENET_VERSION) && ENET_VERSION >= ENET_CUTOFF
			//    mRemote = enet_host_connect(mLocal, &enetAddress, 1, 0);
			//#else
			//    mRemote = enet_host_connect(mLocal, &enetAddress, 1);
			//#endif

			//    ENetEvent event;
			//    if (enet_host_service(mLocal, &event, 10000) <= 0 ||
			//        event.type != ENET_EVENT_TYPE_CONNECT)
			//    {
			//        stop();
			//        return false;
			//    }
			//    return mRemote;

			return true; //ssk
		}

		public void stop()
		{
			//if (mRemote)
			//    enet_peer_disconnect(mRemote, 0);
			//if (mLocal)
			//    enet_host_flush(mLocal);
			//if (mRemote)
			//    enet_peer_reset(mRemote);
			//if (mLocal)
			//    enet_host_destroy(mLocal);

			//mRemote = 0;
			//mLocal = 0;
		}

		public bool isConnected()
		{
			//return mRemote && mRemote->state == ENET_PEER_STATE_CONNECTED;

			return true; //ssk
		}

		//TODO Connection zu Netcomputer zusammenfassen?

		public void send(MessageOut msg)//, bool reliable, uint channel)
		{
			NetworkStream stream=mRemote.GetStream();


			//Länge senden
			ushort lengthPackage=(ushort)msg.getLength();
			byte[] lengthAsByteArray=BitConverter.GetBytes(lengthPackage);
			stream.Write(lengthAsByteArray, 0, (int)lengthAsByteArray.Length); 

			stream.Write(msg.getData(), 0, (int)msg.getLength()); 

			//if (!mRemote) {
			//    LOG_WARN("Can't send message to unconnected host! (" << msg << ")");
			//    return;
			//}

			//gBandwidth->increaseInterServerOutput(msg.getLength());

			//ENetPacket *packet;
			//packet = enet_packet_create(msg.getData(),
			//                            msg.getLength(),
			//                            reliable ? ENET_PACKET_FLAG_RELIABLE : 0);

			//if (packet)
			//    enet_peer_send(mRemote, channel, packet);
			//else
			//    LOG_ERROR("Failure to create packet!");
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
			//            if (event.packet->dataLength >= 2)
			//            {
			//                MessageIn msg((char *)event.packet->data,
			//                              event.packet->dataLength);
			//                gBandwidth->increaseInterServerInput(event.packet->dataLength);
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