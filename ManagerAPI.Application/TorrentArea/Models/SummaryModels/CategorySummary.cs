using QBittorrent.Client;
using System.ComponentModel;

namespace ManagerAPI.Application.TorrentArea.Models.SummaryModels;
public class CategorySummary
{
    public string SummaryMessage { get; set; }
    public Dictionary<string, int> TotalTorrentsByCategory { get; set; } = new();
    public Dictionary<string, string> TorrentsByCategory { get; set; } = new();
    private Dictionary<string, int> SeedingTorrentsByCategory { get; set; } = new();
    private Dictionary<string, int> LeechingTorrentsByCategory { get; set; } = new();
    private Dictionary<string, int> PausedTorrentsByCategory { get; set; } = new();
    private Dictionary<string, int> UnregisteredTorrentsByCategory { get; set; } = new();
    private Dictionary<string, List<string>> CategoryTorrentHashes { get; set; } = new();

    public CategorySummary(List<TorrentInfo> allTorrents, List<string> allCategories)
    {
        SetTorrentsByCategory(allTorrents, allCategories);
        SummaryMessage = $"There are {allCategories.Count()} categories holding {allTorrents.Count()} torrents " +
            $"with {(SeedingTorrentsByCategory.Values.Sum() / allTorrents.Count()) * 100.0} being seeded, " +
            $"{(LeechingTorrentsByCategory.Values.Sum() / allTorrents.Count()) * 100.0} being leeched " +
            $"{(PausedTorrentsByCategory.Values.Sum() / allTorrents.Count()) * 100.0} being leeched " +
            $"and {(UnregisteredTorrentsByCategory.Values.Sum() / allTorrents.Count()) * 100.0} being unregistered " +
            $"making {(SeedingTorrentsByCategory.Values.Sum() + LeechingTorrentsByCategory.Values.Sum() + PausedTorrentsByCategory.Values.Sum() + UnregisteredTorrentsByCategory.Values.Sum() / allTorrents.Count()) * 100.0} of the torrents being accounted for";
    }

    public void SetTorrentsByCategory(List<TorrentInfo> allTorrents, List<string> allCategories)
    {
        foreach(string category in allCategories.OrderBy(category => category))
        {
            var categoryTorrents = allTorrents.Where(torrent => torrent.Category.Equals(category)).ToList();
            TotalTorrentsByCategory[category] = categoryTorrents.Count();
            var seedingCategoryTorrents = categoryTorrents.Where(torrent =>
                torrent.State.Equals(TorrentState.CheckingUpload)
                || torrent.State.Equals(TorrentState.ForcedUpload)
                || torrent.State.Equals(TorrentState.QueuedUpload)
                || torrent.State.Equals(TorrentState.StalledUpload)
                || torrent.State.Equals(TorrentState.Uploading)).ToList();
            SeedingTorrentsByCategory[category] = seedingCategoryTorrents.Count();
            var leechingCategoryTorrents = categoryTorrents.Where(torrent =>
                torrent.State.Equals(TorrentState.CheckingDownload)
                || torrent.State.Equals(TorrentState.ForcedDownload)
                || torrent.State.Equals(TorrentState.QueuedDownload)
                || torrent.State.Equals(TorrentState.StalledDownload)
                || torrent.State.Equals(TorrentState.Downloading)).ToList();
            LeechingTorrentsByCategory[category] = leechingCategoryTorrents.Count();
            var pausedCategoryTorrents = categoryTorrents.Where(torrent =>
                torrent.State.Equals(TorrentState.PausedDownload)
                || torrent.State.Equals(TorrentState.PausedUpload)).ToList();
            PausedTorrentsByCategory[category] = pausedCategoryTorrents.Count();
            var unregisteredCategoryTorrents = categoryTorrents.Where(torrent =>
                torrent.State.Equals(TorrentState.StalledDownload)
                || torrent.State.Equals(TorrentState.StalledUpload)
                && torrent.CurrentTracker == String.Empty).ToList();
            UnregisteredTorrentsByCategory[category] = unregisteredCategoryTorrents.Count();
            TorrentsByCategory[category] = $"{string.Format("{0:n2}", (double.Parse(SeedingTorrentsByCategory[category].ToString()) / double.Parse(TotalTorrentsByCategory[category].ToString())) * 100.0)}% seeding, " +
                $"{string.Format("{0:n2}", (double.Parse(LeechingTorrentsByCategory[category].ToString()) / double.Parse(TotalTorrentsByCategory[category].ToString())) * 100.0)}% leeching, " +
                $"{string.Format("{0:n2}", (double.Parse(PausedTorrentsByCategory[category].ToString()) / double.Parse(TotalTorrentsByCategory[category].ToString())) * 100.0)}% paused, " +
                $"{string.Format("{0:n2}", (double.Parse(UnregisteredTorrentsByCategory[category].ToString()) / double.Parse(TotalTorrentsByCategory[category].ToString())) * 100.0)}% unregistered";
            CategoryTorrentHashes[category] = categoryTorrents.Select(torrent => torrent.Hash).ToList();
        }
    }

    public List<string> GetCategoryTorrentHashes(string category)
    {
        if (CategoryTorrentHashes.ContainsKey(category))
        {
            return CategoryTorrentHashes[category];
        }
        else
        {
            return new();
        }
    }
}
