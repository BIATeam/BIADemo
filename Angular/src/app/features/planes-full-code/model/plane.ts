import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

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
