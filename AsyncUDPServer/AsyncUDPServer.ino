#include "WiFi.h"
#include "AsyncUDP.h"

//const char * ssid = "Flex - Smart Home";
//const char * password = "julia12032009";

const char * ssid = "Flex - Smart Home";
const char * password = "84118411841184118411841184";
int LED_BUILTIN = 2;
String LIGAR = "LIGAR";
String DESLIGAR = "DESLIGAR";
String ALTERNAR = "ALTERNAR";


AsyncUDP udp;

void setup()
{
  Serial.begin(115200);
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);
  IPAddress local_IP(192, 168, 1, 200);
  IPAddress gateway(192, 168, 1, 1);
  IPAddress subnet(255, 255, 0, 0);
  pinMode(LED_BUILTIN, OUTPUT);

  if (!WiFi.config(local_IP, gateway, subnet)) {
    Serial.println("STA Failed to configure");
  }

  if (WiFi.waitForConnectResult() != WL_CONNECTED) {
    Serial.println("WiFi Failed");
    while (1) {
      delay(1000);
    }
  }
  if (udp.listen(9999)) {
    Serial.print("UDP Listening on IP: ");
    Serial.println(WiFi.localIP());
    udp.onPacket([](AsyncUDPPacket packet) {
      Serial.print("UDP Packet Type: ");
      Serial.print(packet.isBroadcast() ? "Broadcast" : packet.isMulticast() ? "Multicast" : "Unicast");
      Serial.print(", From: ");
      Serial.print(packet.remoteIP());
      Serial.print(":");
      Serial.print(packet.remotePort());
      Serial.print(", To: ");
      Serial.print(packet.localIP());
      Serial.print(":");
      Serial.print(packet.localPort());
      Serial.print(", Length: ");
      Serial.print(packet.length());
      Serial.print(", Data: ");


      String response;
      String responseToString;

      for (int i = 0; i <  packet.length(); i++) {
        response += (char)packet.data()[i];
      }

      for (int i = response.lastIndexOf('@') + 1; i < response.lastIndexOf('#'); i++) {
        responseToString += response[i];
      }

      if (responseToString.equalsIgnoreCase(LIGAR)) {
        digitalWrite(LED_BUILTIN, HIGH);
        packet.printf("Got %u bytes of data", LIGAR);
      }

      if (responseToString.equalsIgnoreCase(DESLIGAR)) {
        digitalWrite(LED_BUILTIN, LOW);
        packet.printf("Got %u bytes of data", DESLIGAR);
      }
      if (responseToString.equalsIgnoreCase(ALTERNAR)) {
        digitalWrite(LED_BUILTIN, !digitalRead(LED_BUILTIN));
        packet.printf("Got %u bytes of data", DESLIGAR);
      }
      //reply to the client
      packet.printf("Got %u bytes of data", packet.localIP());
    });
  }
}

void loop()
{
  delay(1000);
  //Send broadcast
  udp.broadcast("Send broadcast");
}
