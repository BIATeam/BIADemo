import { BiaDatabasePlatformBridge } from '../bia.platform-bridge';

export class BiaDatabaseElectronBridge implements BiaDatabasePlatformBridge {
  async runQuery(query: string, params: any[]): Promise<number> {
    return await (window as any).biaElectronBridge?.database?.runQuery(
      query,
      params
    );
  }

  async getQuery<T>(query: string, params: any[]): Promise<T> {
    return await (window as any).biaElectronBridge?.database?.getQuery(
      query,
      params
    );
  }
}
