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
            username: ''
        };
    }

    componentDidMount() {
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (authCookie) {
            const authCookieValue = authCookie.split('=')[1];
            this.setState({ username: authCookieValue });

            fetch(`/api/user/GetUserDetails/${authCookieValue}`)
                .then(response => response.json())
                .then(data => {
                    this.setState({
                        isLoggedIn: true,
                        isLoading: false,
                        username: data.username,
                        name: data.name,
                        surname: data.surname,
                        email: data.email,
                        phone: data.phone,
                        referal_code: data.referal_code,
                        date: new Date().toLocaleDateString(),
                        loyalty_progress: data.loyalty_progress,
                    });
                })
                .catch(error => {
                    console.error('Error fetching user details:', error);
                    this.setState({ isLoggedIn: false });
                });

            // Fetch the leaderboard data
            this.fetchLeaderboardData();
        } else {
            // Authentication cookie not present
            this.setState({ isLoggedIn: false });
        }
    }

    fetchLeaderboardData() {
        fetch('api/leaderboard/GetTopUsers')
            .then(response => response.json())
            .then(data => {
                console.log("Fetched data:", data); // Debugging log
                this.setState({ users: data, isLoading: false });
            })
            .catch(error => {
                console.error('Error fetching leaderboard data:', error);
                this.setState({ isLoading: false });
            });
    }


    static renderLeaderboardTable(users) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Username</th>
                        <th>Loyalty Progress</th>
                    </tr>
                </thead>
                <tbody>
                    {users.map(user =>
                        <tr key={user.id}>
                            <td>{user.username}</td>
                            <td>{user.loyalty_progress}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        const { isLoggedIn, isLoading, users } = this.state;

        // Redirect to /fetch-account if the user is not logged in
        if (!isLoggedIn) {
            window.location.href = '/fetch-account';
            return null; // This is important to prevent the component from rendering
        }

        let leaderboardContents = isLoading
            ? <p><em>Loading leaderboard...</em></p>
            : FetchLeaderboard.renderLeaderboardTable(users);

        return (
            <div>
                <h2>Leaderboard</h2>
                {leaderboardContents}
            </div>
        );
    }
}
