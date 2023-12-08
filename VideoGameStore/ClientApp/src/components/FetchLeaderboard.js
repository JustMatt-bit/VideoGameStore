import React, { Component } from 'react';
import { Redirect } from 'react-router-dom';
import './NavMenu.css';

export class FetchLeaderboard extends Component {
    static displayName = FetchLeaderboard.name;

    constructor(props) {
        super(props);

        this.state = {
            isLoggedIn: true,
        };
    }

    componentDidMount() {
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (authCookie) {
            const authCookieValue = authCookie.split('=')[1];
            this.state.username = authCookieValue;

            // Make API call to fetch user details
            fetch(`/api/user/GetUserDetails/${authCookieValue}`)
                .then(response => response.json())
                .then(data => {
                    // Update the state with user information
                    this.setState({
                        isLoggedIn: true,
                        isLoading: false,
                        name: data.name,
                        surname: data.surname,
                        email: data.email,
                        phone: data.phone,
                        referal_code: data.referal_code,
                        date: new Date().toLocaleDateString(),
                    });
                })
                .catch(error => {
                    console.error('Error fetching user details:', error);
                    // Handle error, e.g., redirect to login page
                    this.setState({ isLoggedIn: false });
                });
        } else {
            // Authentication cookie not present
            this.setState({ isLoggedIn: false });
        }
    }

    render() {
        const { isLoggedIn } = this.state;

        // Redirect to /fetch-account if the user is not logged in
        if (!isLoggedIn) {
            window.location.href = '/fetch-account';
            return null; // This is important to prevent the component from rendering
        }

        return (
            <div>
                <div>
                    <h2>Leaderboard</h2>
                </div>

                <h2>This is the leaderboard</h2>
            </div>
        );
    }
}
