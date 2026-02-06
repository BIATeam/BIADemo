import { OptionDto } from 'packages/bia-ng/models/public-api';

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
