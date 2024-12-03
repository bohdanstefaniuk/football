class LeagueCard extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: 'open' });
    }

    connectedCallback() {
        const name = this.getAttribute('name');
        const country = this.getAttribute('country');
        this.shadowRoot.innerHTML = `
                    <style>
                        .league {
                            margin-bottom: 24px;
                        }
                        h2 {
                            font-size: 18px;
                            margin: 0 0 12px 0;
                        }
                        .matches-container {
                            display: flex;
                            overflow-x: auto;
                            gap: 12px;
                            padding: 4px 0;
                            -webkit-overflow-scrolling: touch;
                            flex-wrap: nowrap;
                            scroll-behavior: smooth;
                            overflow-y: hidden;
                            scroll-snap-type: x mandatory;
                        }
                    </style>
                    <div class="league">
                        <h2>${name} (${country})</h2>
                        <div class="matches-container">
                            <slot></slot>
                        </div>
                    </div>
                `;
    }
}

customElements.define('league-card', LeagueCard);