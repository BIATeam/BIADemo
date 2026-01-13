import { OptionDto } from '@bia-team/bia-ng/models';

export interface Plane {
  id: number;
  msn: string;
  isActive: boolean;
  lastFlightDate: Date;
  deliveryDate: Date;
  syncTime: string;
  capacity: number;
  siteId: number;
  connectingAirports: OptionDto[];
  planeType: OptionDto | null;
}
