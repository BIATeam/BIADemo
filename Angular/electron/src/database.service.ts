import sqlite3 from 'sqlite3';

export class DatabaseService {
  dbPath: string;
  db: sqlite3.Database;

  constructor(dbPath: string) {
    this.dbPath = dbPath;
    this.db = null;
  }

  initialize() {
    this.db = new sqlite3.Database(this.dbPath, err => {
      if (err) {
        console.error('Failed to connect to the database:', err);
        throw err;
      }
      console.log('Connected to SQLite database.');
    });

    this.createTables();
  }

  private createTables() {
    const query = `
            CREATE TABLE IF NOT EXISTS users (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL,
                email TEXT NOT NULL UNIQUE
            );
        `;
    this.db.run(query, err => {
      if (err) {
        console.error('Failed to create tables:', err);
      } else {
        console.log('Tables created successfully.');
      }
    });
  }

  runQuery(query, params = []) {
    return new Promise((resolve, reject) => {
      this.db.run(query, params, function (err) {
        if (err) {
          reject(err);
        } else {
          resolve({ id: this.lastID });
        }
      });
    });
  }

  getQuery(query, params = []) {
    return new Promise((resolve, reject) => {
      this.db.all(query, params, (err, rows) => {
        if (err) {
          reject(err);
        } else {
          resolve(rows);
        }
      });
    });
  }

  close() {
    this.db.close(err => {
      if (err) {
        console.error('Failed to close the database:', err);
      } else {
        console.log('Database connection closed.');
      }
    });
  }
}
