import React, { Component } from 'react';
import './NavMenu.css';

export class FetchLeaderboard extends Component {
    static displayName = FetchLeaderboard.name;

    constructor(props) {
        super(props);

        this.state = {
            isLoggedIn: true,
            isLoading: true,
            users: [],
            originalUsers: [],
            username: '',
            userPosition: null,
            showingUserPosition: false,
        };
    }

    componentDidMount() {
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (authCookie) {
            const username = authCookie.split('=')[1];
            this.setState({ username });

            this.fetchLeaderboardData();
        } else {
            this.setState({ isLoggedIn: false });
        }
    }

    fetchLeaderboardData() {
        fetch('api/leaderboard/GetTopUsers')
            .then(response => response.json())
            .then(data => {
                this.setState({ originalUsers: data, users: data, isLoading: false });
            })
            .catch(error => {
                console.error('Error fetching leaderboard data:', error);
                this.setState({ isLoading: false });
            });
    }


    static renderLeaderboardTable(users, userPosition) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Position</th>
                        <th>Username</th>
                        <th>Loyalty Progress</th>
                    </tr>
                </thead>
                <tbody>
                    {users.map((user, index) =>
                        <tr key={user.username}>
                            <td>{userPosition || (index + 1)}</td>
                            <td>{user.username}</td>
                            <td>{user.loyalty_progress}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }


    render() {
        const { isLoggedIn, isLoading, users, userPosition, showingUserPosition } = this.state;

        // Redirect to /fetch-account if the user is not logged in
        if (!isLoggedIn) {
            window.location.href = '/fetch-account';
            return null; // This is important to prevent the component from rendering
        }

        let leaderboardContents = isLoading
            ? <p><em>Loading leaderboard...</em></p>
            : FetchLeaderboard.renderLeaderboardTable(users, showingUserPosition ? userPosition : null);

        let userPositionDisplay = showingUserPosition && userPosition ? (
            <div>
                <p>Your Position: {userPosition}</p>
            </div>
        ) : null;

        return (
            <div>
                <h2>Leaderboard</h2>
                <button onClick={this.findUserPosition} className="leaderboard-button">
                    {showingUserPosition ? "Back" : "Show My Position"}
                </button>
                {userPositionDisplay}
                {leaderboardContents}
            </div>
        );

    }

    findUserPosition = () => {
        const { showingUserPosition, originalUsers, username } = this.state;

        if (!showingUserPosition) {
            const position = originalUsers.findIndex(user => user.username === username) + 1;
            const filteredUser = originalUsers.find(user => user.username === username);
            this.setState({
                userPosition: position,
                users: filteredUser ? [filteredUser] : [],
                showingUserPosition: true
            });
        } else {
            this.setState({ users: originalUsers, showingUserPosition: false, userPosition: null });
        }
    }
}
