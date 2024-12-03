class MatchCard extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: 'open' });
    }

    connectedCallback() {
        const homeTeam = this.getAttribute('home-team');
        const homeTeamCrest = this.getAttribute('home-team-crest');

        const awayTeam = this.getAttribute('away-team');
        const awayTeamCrest = this.getAttribute('away-team-crest');

        const matchTime = new Date(this.getAttribute('match-time'));

        const winOdds = this.getAttribute('win-odds');
        const drawOdds = this.getAttribute('draw-odds');
        const loseOdds = this.getAttribute('lose-odds');

        const homeTeamScore = this.getAttribute('home-team-score');
        const awayTeamScore = this.getAttribute('away-team-score');
        const winner = this.getAttribute('winner');

        const country = this.getAttribute('country');

        this.shadowRoot.innerHTML = `
                    <style>
                        .match-card {
                            min-width: 280px;
                            width: 280px;
                            padding: 16px;
                            border: 1px solid #eaeaea;
                            border-radius: 12px;
                            box-shadow: 0 2px 4px rgba(0,0,0,0.05);
                            background: white;
                            scroll-snap-align: start;
                            user-select: none;
                            -webkit-tap-highlight-color: transparent;
                        }
                        .time {
                            color: #666;
                            font-size: 14px;
                            margin-bottom: 8px;
                        }
                        .teams {
                            margin-bottom: 8px;
                        }
                        .team {
                            display: flex;
                            align-items: center;
                            gap: 8px;
                            font-size: 16px;
                            font-weight: 600;
                            margin-bottom: 4px;
                        }
                        .winner-label {
                            background-color: green;
                            color: white;
                            font-size: 12px;
                            font-weight: 600;
                            padding: 2px 6px;
                            border-radius: 4px;
                            margin-left: 8px;
                        }
                        .team .score {
                            font-size: 18px;
                            font-weight: 600;
                            margin-left: auto;
                        }
                        .team img {
                            width: 24px;
                            height: 24px;
                        }
                        .odds {
                            display: flex;
                            gap: 8px;
                        }
                        .odd {
                            font-size: 14px;
                            font-weight: 600;
                            text-align: center;
                            flex: 1;
                            padding: 8px;
                            border: 1px solid #eaeaea;
                            border-radius: 8px;
                            background: #f9f9f9;
                        }
                        .odd-type {
                            font-size: 12px;
                            color: #666;
                        }
                        .odd-value {
                            font-size: 16px;
                            font-weight: 600;
                        }
                        .odd p {
                            margin: 0;
                        }
                    </style>
                    <div class="match-card">
                        <div class="time">${matchTime.toLocaleString()} - (${country})</div>
                        <div class="teams">
                            <div class="team">
                                <img src="${homeTeamCrest}" alt="${homeTeam} Crest">
                                <span class="name">${homeTeam}</span>
                                ${winner === "HomeTeam" ? `<span class="winner-label">Winner</span>` : ''}
                                ${homeTeamScore ? `<span class="score">${homeTeamScore}</span>` : ''}
                            </div>
                            <div class="team">
                                <img src="${awayTeamCrest}" alt="${awayTeam} Crest">
                                <span class="name">${awayTeam}</span>
                                ${winner === "AwayTeam" ? `<span class="winner-label">Winner</span>` : ''}
                                ${awayTeamScore ? `<span class="score">${awayTeamScore}</span>` : ''}
                            </div>
                        </div>
                        <div class="odds">
                            <div class="odd">
                                <p class="odd-type">Win</p>
                                <p class="odd-value">${winOdds}</p>
                            </div>
                            <div class="odd">
                                <p class="odd-type">Draw</p>
                                <p class="odd-value">${drawOdds}</p>
                            </div>
                            <div class="odd">
                                <p class="odd-type">Lose</p>
                                <p class="odd-value">${loseOdds}</p>
                            </div>
                        </div>
                    </div>
                `;
    }
}

customElements.define('match-card', MatchCard);