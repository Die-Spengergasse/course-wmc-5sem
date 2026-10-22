import "./style.css";

// ---------------------------------------------------------------------------
// Types
// ---------------------------------------------------------------------------

type Role = "tank" | "damage" | "support";
type MatchStatus = "scheduled" | "live" | "finished";

interface Stats {
    kills: number;
    deaths: number;
    assists: number;
}

interface Clan {
    tag: string;
    joinedAt: string;
}

interface Player {
    gamertag: string;
    realName?: string;
    role: Role;
    stats: Stats;
    clan?: Clan;
}

interface Team {
    tag: string;
    name: string;
    players: Player[];
}

interface Score {
    teamA: number;
    teamB: number;
}

interface Match {
    id: number;
    round: number;
    teamA: string;
    teamB: string;
    status: MatchStatus;
    score: Score | null;
    mvp?: string;
}

interface Venue {
    school: string;
    room: string;
}

interface Tournament {
    id: number;
    name: string;
    game: string;
    startsAt: string;
    venue: Venue;
    teams: Team[];
    matches: Match[];
}

// ---------------------------------------------------------------------------
// Data (in a real app, this comes from GET /api/tournaments/7)
// ---------------------------------------------------------------------------

const tournament: Tournament = {
    id: 7,
    name: "Spengi Cup",
    game: "Arena Clash",
    startsAt: "2026-11-14T09:00:00",
    venue: { school: "HTL Spengergasse", room: "C3.07" },
    teams: [
        {
            tag: "5AAIF",
            name: "Null Pointer Ninjas",
            players: [
                {
                    gamertag: "byteMe",
                    realName: "Lena",
                    role: "tank",
                    stats: { kills: 12, deaths: 4, assists: 9 },
                    clan: { tag: "NPN", joinedAt: "2025-09-02" }
                },
                {
                    gamertag: "segfault",
                    role: "damage",
                    stats: { kills: 21, deaths: 0, assists: 3 }
                },
                {
                    gamertag: "404brain",
                    realName: "Jonas",
                    role: "support",
                    stats: { kills: 2, deaths: 7, assists: 25 }
                }
            ]
        },
        {
            tag: "5BAIF",
            name: "Ctrl Alt Defeat",
            players: [
                {
                    gamertag: "sudo_sara",
                    realName: "Sara",
                    role: "damage",
                    stats: { kills: 17, deaths: 9, assists: 4 },
                    clan: { tag: "CAD", joinedAt: "2024-11-20" }
                },
                {
                    gamertag: "lagMaster",
                    role: "tank",
                    stats: { kills: 6, deaths: 11, assists: 8 }
                },
                {
                    gamertag: "pingu",
                    realName: "Emre",
                    role: "support",
                    stats: { kills: 3, deaths: 5, assists: 19 },
                    clan: { tag: "CAD", joinedAt: "2025-01-15" }
                }
            ]
        }
    ],
    matches: [
        {
            id: 1,
            round: 1,
            teamA: "5AAIF",
            teamB: "5BAIF",
            status: "finished",
            score: { teamA: 13, teamB: 9 },
            mvp: "segfault"
        },
        {
            id: 2,
            round: 2,
            teamA: "5BAIF",
            teamB: "5AAIF",
            status: "live",
            score: { teamA: 4, teamB: 4 }
        },
        {
            id: 3,
            round: 3,
            teamA: "5AAIF",
            teamB: "5BAIF",
            status: "scheduled",
            score: null
        }
    ]
};

// The role selected in the filter. "all" shows every player.
let selectedRole: Role | "all" = "all";

// ---------------------------------------------------------------------------
// Calculations
// ---------------------------------------------------------------------------

/**
 * Calculates the KDA value: (kills + assists) / deaths.
 * A player with 0 deaths counts as if they had 1 death.
 */
function calcKda(stats: Stats): number {
    const deaths = Math.max(stats.deaths, 1);
    return (stats.kills + stats.assists) / deaths;
}

/** Adds up the kills of all players in a team. */
function totalKills(team: Team): number {
    return team.players.reduce((sum, player) => sum + player.stats.kills, 0);
}

/** Finds a team by its tag, e.g. "5AAIF". Throws if the team does not exist. */
function findTeam(tag: string): Team {
    const team = tournament.teams.find(t => t.tag === tag);
    if (team === undefined) {
        throw new Error(`Team ${tag} not found.`);
    }
    return team;
}

/** Searches a player by gamertag in all teams. */
function findPlayer(gamertag: string): Player | undefined {
    for (const team of tournament.teams) {
        const player = team.players.find(p => p.gamertag === gamertag);
        if (player !== undefined) {
            return player;
        }
    }
    return undefined;
}

/** Returns the tag of the winning team or "draw". */
function winner(match: Match, score: Score): string {
    if (score.teamA > score.teamB) return match.teamA;
    if (score.teamB > score.teamA) return match.teamB;
    return "draw";
}

// ---------------------------------------------------------------------------
// Formatting
// ---------------------------------------------------------------------------

/** Formats one player, e.g. "byteMe [NPN] tank, KDA 5.25". */
function formatPlayer(player: Player): string {
    const clanTag = player.clan?.tag ?? "-";
    return `${player.gamertag} [${clanTag}] ${player.role}, KDA ${calcKda(player.stats).toFixed(2)}`;
}

/** Formats a match, e.g. "Round 1: Null Pointer Ninjas 13:9 Ctrl Alt Defeat -> winner: 5AAIF". */
function formatMatch(match: Match): string {
    const nameA = findTeam(match.teamA).name;
    const nameB = findTeam(match.teamB).name;
    if (match.score === null) {
        return `Round ${match.round}: ${nameA} vs ${nameB}`;
    }
    let line = `Round ${match.round}: ${nameA} ${match.score.teamA}:${match.score.teamB} ${nameB}`;
    if (match.status === "finished") {
        line += ` -> winner: ${winner(match, match.score)}`;
    }
    return line;
}

// ---------------------------------------------------------------------------
// DOM
// ---------------------------------------------------------------------------

/** Finds an element with querySelector and throws if it does not exist. */
function getElement<T extends Element>(selector: string): T {
    const element = document.querySelector<T>(selector);
    if (element === null) {
        throw new Error(`Element ${selector} not found.`);
    }
    return element;
}

const titleElement = getElement<HTMLHeadingElement>("#title");
const venueElement = getElement<HTMLParagraphElement>("#venue");
const roleFilter = getElement<HTMLSelectElement>("#roleFilter");
const teamsElement = getElement<HTMLDivElement>("#teams");
const matchesElement = getElement<HTMLUListElement>("#matches");
const mvpElement = getElement<HTMLParagraphElement>("#mvp");

/** Shows name, game and room of the tournament. */
function renderHeader(): void {
    titleElement.textContent = `${tournament.name} (${tournament.game})`;
    venueElement.textContent = `${tournament.venue.school}, room ${tournament.venue.room}`;
}

/** Shows every team with the players of the selected role. */
function renderTeams(): void {
    teamsElement.replaceChildren();
    for (const team of tournament.teams) {
        const heading = document.createElement("h3");
        heading.textContent = `${team.name} (${team.tag}), total kills: ${totalKills(team)}`;

        const list = document.createElement("ul");
        const players = team.players.filter(p => selectedRole === "all" || p.role === selectedRole);
        for (const player of players) {
            const item = document.createElement("li");
            item.textContent = formatPlayer(player);
            list.append(item);
        }
        teamsElement.append(heading, list);
    }
}

/** Shows all matches and the MVP of round 1. */
function renderMatches(): void {
    matchesElement.replaceChildren();
    for (const match of tournament.matches) {
        const item = document.createElement("li");
        item.textContent = formatMatch(match);
        item.className = match.status;
        matchesElement.append(item);
    }

    const mvpTag = tournament.matches[0]?.mvp;
    const mvp = mvpTag === undefined ? undefined : findPlayer(mvpTag);
    mvpElement.textContent = mvp === undefined ? "No MVP yet." : `MVP of round 1: ${formatPlayer(mvp)}`;
}

roleFilter.addEventListener("change", () => {
    // Look the value up in a typed list, so selectedRole only gets allowed values.
    const options: (Role | "all")[] = ["all", "tank", "damage", "support"];
    selectedRole = options.find(option => option === roleFilter.value) ?? "all";
    renderTeams();
});

renderHeader();
renderTeams();
renderMatches();
