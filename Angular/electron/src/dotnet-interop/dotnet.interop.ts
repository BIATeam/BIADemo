import edge from 'electron-edge-js';

export class DotnetInterop {
  execute() {
    const dllName = 'Electron.dll';
    const dllDirectory =
      'C:\\sources\\Local\\Electron\\Electron\\bin\\Release\\net8.0\\publish\\win-x64';
    const dllPath = `${dllDirectory}\\${dllName}`;
    const namespace = 'Electron.PublicAPI';

    process.env.EDGE_USE_CORECLR = '1';
    process.env.EDGE_APP_ROOT = dllDirectory;

    const getRandomNumber = edge.func({
      assemblyFile: dllPath,
      typeName: namespace,
      methodName: 'GetRandomNumber',
    });

    const getRandomNumberStatic = edge.func({
      assemblyFile: dllPath,
      typeName: namespace,
      methodName: 'GetRandomNumberStatic',
    });

    const concatArgs = edge.func({
      assemblyFile: dllPath,
      typeName: namespace,
      methodName: 'ConcatArgs',
    });

    const doAsync = edge.func({
      assemblyFile: dllPath,
      typeName: namespace,
      methodName: 'DoAsync',
    });

    getRandomNumber(undefined, (err, res) => {
      if (err) throw err;
      console.log('getRandomNumber', res);
    });

    getRandomNumberStatic(undefined, (err, res) => {
      if (err) throw err;
      console.log('getRandomNumberStatic', res);
    });

    concatArgs({ arg1: 'test', arg2: '2' }, (err, res) => {
      if (err) throw err;
      console.log('concatArgs', res);
    });

    doAsync(undefined, (err, res) => {
      if (err) throw err;
      console.log('doAsync', res);
    });
  }
}
