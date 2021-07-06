# Client for hub (Worker Feature)
This file explains what to use the the hub for client feature in your V3 project.

## Prerequisite

### Knowledge to have:
* [SignalR introduction](https://docs.microsoft.com/fr-fr/aspnet/signalr/overview/getting-started/introduction-to-signalr)

## Overview
The activation of this feature in the worker give the possibility to send message to all connected Clients directly from the worker service.
Concretly the service is connected to a front taht propagate the message. 

## Activation
* bianetconfig.json

In the BIANet Section add:
```Json
    "WorkerFeatures": {
      "HubForClients": {
        "Activate": true,
      }
    },
```
* bianetconfig.Development.json
```Json
    "WorkerFeatures": {
      "HubForClients": {
        "SignalRUrl": "http://localhost/BIADemo/WebApi/HubForClients"
      }
    },
```
* bianetconfig.ENV.json (4 files not Development)
In the BIANet Section add:
```Json
    "WorkerFeatures": {
      "HubForClients": {
        "SignalRUrl": "https://dmeu-[ENV].electrical-power.thebiadevcompany/BIADemo/WebApi/HubForClients"
      }
    },
```
replace [ENV] by the corresponding env of the file (int, uat, prd or pra)

## Usage
```csharp
    using BIA.Net.Core.WorkerService.Features.HubForClients;
    ...
    HubForClientsService.SendMessage("refresh-planes", "");
```