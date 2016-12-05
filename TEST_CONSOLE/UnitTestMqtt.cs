﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using APGW_DOTNET;
using Common;
using System.Diagnostics;
using System.Threading;

namespace TEST_APGW_CORE
{
    [TestFixture]
    class UnitTestMqtt
    {
        public UnitTestMqtt()
        {
        }

        [Test]
        public void createMqttClient()
        {
            MQTT mqtt_client = new MQTT("mere-vase-5982.staging.nanoscaleapi.io",1883);

            Assert.IsNotNull(mqtt_client);
        }

        [Test]
        public void connectToBroker()
        {
            MQTT mqtt_client = new MQTT("mere-vase-5982.staging.nanoscaleapi.io",1883);
            mqtt_client.Connect("123445", "shassan@anypresence.com,PushMessagesAPI,push,mqtt", "password");
            Assert.IsTrue(mqtt_client.isConnected());
        }

        [Test]
        public void subscribeChannel()
        {
            MQTT mqtt_client = new MQTT("mere-vase-5982.staging.nanoscaleapi.io", 1883);
            Assert.IsNotNull(mqtt_client);
            mqtt_client.Connect("123456", "shassan@anypresence.com,PushMessagesAPI,push,mqtt", "password");
            Assert.IsTrue(mqtt_client.isConnected());
            mqtt_client.Subscribe(new string[] { "/dotnet_channel4/topic1/" }, (args) =>
            {
                var lArgs =(subscribedEventArgs)args;
                Assert.IsNotNull(lArgs);
                Assert.Positive(lArgs.messageId);
            });

        }

        [Test]
        public void unsubscribeChannel()
        {
            MQTT mqtt_client = new MQTT("mere-vase-5982.staging.nanoscaleapi.io", 1883);
            mqtt_client.Connect("123456", "shassan@anypresence.com,PushMessagesAPI,push,mqtt", "password");
            mqtt_client.unSubscribe(new string[] { "dotnet_channel" },(value)=>
            {
                Assert.IsNotNull(value);
    
            });
        }

        [Test]
        public void publishMessage()
        {
            MQTT mqtt_client = new MQTT("mere-vase-5982.staging.nanoscaleapi.io", 1883);
            mqtt_client.Connect("1234567", "shassan@anypresence.com,PushMessagesAPI,push,mqtt", "password");
            Assert.IsTrue(mqtt_client.isConnected());
            mqtt_client.Subscribe(new string[] { "/dotnet_channel4/topic1/" },(args)=>
            {
                var lArgs = (subscribedEventArgs)args;
                Assert.IsNotNull(lArgs);
                Assert.Positive(lArgs.messageId);

            },(args)=>
            {
                var lArgs = (publishEventArgs)args;
                Assert.Equals(Encoding.UTF8.GetString(lArgs.message),"message");

            });
            
            mqtt_client.Publish("dotnet_channel4/topic1/", "message", (args) =>
            {
                Assert.IsTrue(((publishedEventArgs)args).isPublished);
            });

        }
       
    }
}
