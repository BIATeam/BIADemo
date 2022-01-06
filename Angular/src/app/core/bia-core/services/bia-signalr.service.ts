import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { TargetedFeature } from 'src/app/shared/bia-shared/model/signalR';
import { environment } from 'src/environments/environment';

/**
 * Service managing the SignalR connection.
 * You can add specific events management by doing the following:
 * - Add a SignalRService parameter to the constructor of your component (for dependency injection)
 * - Use this injected instance of SignalRService to add new events:
 *     this.signalRService.getHubConnection().on('MyEventName', () => {
 *       this.store.select(getMyEventHandler).pipe(first()).subscribe(
 *         (myEventData) => {
 *             // Do what you need here...
 *         }
 *       );
 *     });
 */
@Injectable()
export class BiaSignalRService {
  /**
   * Object managing the SignalR connection.
   * You can access it through the 'getHubConnection()' method, so that every component/feature can add new event management,
   * without having to redefine every needed event and data structure in the domain.
   */


  private readonly hubConnection: HubConnection;
  private methods: string[];
  private isStarting: boolean;
  private isStarted: boolean;
  private targetedFeatures: TargetedFeature[];

  /**
   * Constructor.
   */
  public constructor() {
    this.hubConnection = new HubConnectionBuilder().withUrl(environment.hubUrl).build();

    this.configureConnection();

    this.methods = [];
    this.isStarting = false;
    this.isStarted = false;
    this.targetedFeatures = [];
  }

  public addMethod(methodName: string, newMethod: (...args: any[]) => void): void {
    this.hubConnection.on(methodName, newMethod);
    if (this.methods.indexOf(methodName) < 0) {
      this.methods.push(methodName);
    }
    if (!this.isStarting) {
      this.isStarting = true;
      this.startConnection();
    }
  }
  public removeMethod(methodName: string): void {
    this.hubConnection.off(methodName);
    if (this.methods.indexOf(methodName) > -1) {
      this.methods.splice(this.methods.indexOf(methodName), 1);
    }

    // We wait 500ms before stop the connection, to not do it if it is use in next screen
    setTimeout(() => {
      if (this.methods.length === 0) {
        this.isStarting = false;
        this.hubConnection.stop();
      }
    }, 500);
  }

  public joinGroup(targetedFeature: TargetedFeature) {
    if (this.targetedFeatures.indexOf(targetedFeature) === -1 ) {
      this.targetedFeatures.push(targetedFeature);
    }
    if (this.isStarted) {
      this.invokeJoinGroup(targetedFeature);
    }
  }

  private invokeJoinGroup(targetedFeature: TargetedFeature) {
    this.hubConnection.invoke('JoinGroup', JSON.stringify(targetedFeature))
      .then(() => {
          console.log('%c [SignalRService] Join Group ' + targetedFeature.parentKey + '>' + targetedFeature.featureName, 'color: blue; font-weight: bold');
        }
      )
      .catch(err => {
        console.log(err);
      });
  }

  public leaveGroup(targetedFeature: TargetedFeature) {
    const index = this.targetedFeatures.indexOf(targetedFeature);
    if (index > -1) {
      this.targetedFeatures.slice(index);
    }
    if (this.isStarted) {
      this.invokeLeaveGroup(targetedFeature);
    }
  }

  private invokeLeaveGroup(targetedFeature: TargetedFeature) {
    this.hubConnection.invoke('LeaveGroup', JSON.stringify(targetedFeature))
      .then(() => {
          console.log('%c [SignalRService] Leave Group ' + targetedFeature.parentKey + '>' + targetedFeature.featureName, 'color: blue; font-weight: bold');
        }
      )
      .catch(err => {
        console.log(err);
      });
  }

  /**
   * Configure the connection behavior.
   */
  private configureConnection(): void {
    this.hubConnection.onclose(async () => {
      if (this.isStarting) {
        console.log('%c [SignalRService] Hub connection closed. Try restarting it...', 'color: red; font-weight: bold');
        setTimeout((e) => {
          this.startConnection();
        }, 5000);
      } else {
        this.isStarted = false;
        console.log('%c [SignalRService] Hub connection stoped', 'color: blue; font-weight: bold');
      }
    });
  }

  /**
   * Start the SignalR connection.
   */
  private startConnection(): void {
    this.hubConnection
      .start()
      .then(() => {
        this.isStarted = true;
        this.targetedFeatures.forEach(element => {
          this.invokeJoinGroup(element);
        });
        console.log('%c [SignalRService] Hub connection started', 'color: blue; font-weight: bold');
      })
      .catch((err: string) => {
        if (this.isStarting) {
          console.log(
            '%c [SignalRService] Error while establishing connection, retrying...' + err,
            'color: red; font-weight: bold'
          );
          setTimeout((e) => {
            this.startConnection();
          }, 5000);
        } else {
          console.log(
            '%c [SignalRService] Hub connection stoped before establishing connection.',
            'color: blue; font-weight: bold'
          );
        }
      });
  }
}
