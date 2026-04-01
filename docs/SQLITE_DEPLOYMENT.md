# SQLite deployment (local POS)

## Database file

The connection string in `appsettings.json` defaults to `Data Source=pos.db` (relative path). The file is created next to the application executable after the first run (migrations apply automatically).

## Backup

1. Close the POS application (or ensure no open handles on `pos.db`).
2. Copy `pos.db` to a safe location (dated backup folder, network drive, or removable media).

Restore by replacing `pos.db` with the backup while the app is closed, then start the POS app again.

## Notes

- WAL mode: SQLite may create `-wal` and `-shm` companion files; copy those together with `pos.db` for a consistent backup while the app is not running.
- For production, schedule regular backups and test restores on a spare machine.
