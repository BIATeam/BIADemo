import { BiaOsBridgeDatabase } from '../bia-os-bridge';

export class BiaOsBridgeElectronDatabase implements BiaOsBridgeDatabase {
  async runQuery(query: string, params: any[]): Promise<number> {
    console.log('runQuery', window);
    return await (window as any).electronBridge?.database?.runQuery(
      query,
      params
    );
  }

  async getQuery<T>(query: string, params: any[]): Promise<T> {
    return await (window as any).electronBridge?.database?.getQuery(
      query,
      params
    );
  }
}
