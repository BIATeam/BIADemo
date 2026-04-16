import { Notification } from 'packages/bia-ng/features/public-api';

export interface SpecificNotification extends Notification {
  acknowledgedAt?: Date;
}
