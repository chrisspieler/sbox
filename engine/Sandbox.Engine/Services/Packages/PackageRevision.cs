using System.Threading;

namespace Sandbox;

internal sealed class PackageRevision : Package.IRevision
{
	long Package.IRevision.VersionId => AssetVersionId;
	long Package.IRevision.FileCount => FileCount;
	long Package.IRevision.TotalSize => TotalSize;
	DateTimeOffset Package.IRevision.Created => Created;
	int Package.IRevision.EngineVersion => EngineVersion;
	ManifestSchema Package.IRevision.Manifest => _manifest;

	public long FileCount { get; set; }
	public long AssetVersionId { get; set; }
	public long TotalSize { get; set; }
	public string ManifestUrl { get; set; }
	public string Summary { get; set; }
	public DateTimeOffset Created { get; set; }
	public int EngineVersion { get; set; }
	public string Meta { get; set; }
	public string Changes { get; set; }

	ManifestSchema _manifest;
}
