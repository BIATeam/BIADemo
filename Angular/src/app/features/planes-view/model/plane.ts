export interface Plane {
  id: number;
  msn: string;
  isActive: boolean;
  firstFlightDate: Date;
  firstFlightTime: Date;
  lastFlightDate: Date;
  capacity: number;
}
