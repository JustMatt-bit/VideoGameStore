import React, { Component } from 'react';
import './NavMenu.css';

export class EditAccount extends Component {
    static displayName = EditAccount.name;

    constructor(props) {
        super(props);

        this.state = {
            isLoggedIn: true,
            username: '',
            password: '',
            name: '',
            surname: '',
            email: '',
            phone: '',
            referal_code: '',
            date: new Date().toLocaleDateString(),
            statusMessage: '', // Add status message state
        };
    }

    handleInputChange = (event) => {
        const { name, value } = event.target;
        this.setState({ [name]: value });
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

    handleSubmit = async (event) => {
        event.preventDefault();

        // Make API call to update user information
        try {
            const response = await fetch("/api/user/edit", {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(this.state),
            });

            if (response.ok) {
                console.log('User information updated successfully', response.ok);
                // Set success status message
                this.setState({ statusMessage: 'User information updated successfully' });
            } else {
                console.error('Failed to update user information');
                // Set error status message
                this.setState({ statusMessage: 'Failed to update user information' });
            }
        } catch (error) {
            console.error('Error during API call:', error);
            // Set error status message
            this.setState({ statusMessage: 'Error during API call' });
        }
    };

    render() {
        const { isLoggedIn, statusMessage } = this.state;

        // Redirect to /fetch-account if the user is not logged in
        if (!isLoggedIn) {
            window.location.href = '/fetch-account';
            return null; // This is important to prevent the component from rendering
        }
        return (
            <div>
                <h2>Edit account</h2>
                {statusMessage && <p>{statusMessage}</p>}

                <form onSubmit={this.handleSubmit}>



                    <div>
                        <label>Password:</label><br />
                        <input
                            type="password"
                            name="password"
                            value={this.state.password}
                            onChange={this.handleInputChange}
                        />
                    </div><br />

                    <div>
                        <label>Name:</label><br />
                        <input
                            type="text"
                            name="name"
                            value={this.state.name}
                            onChange={this.handleInputChange}
                        />
                    </div><br />

                    <div>
                        <label>Surname:</label><br />
                        <input
                            type="text"
                            name="surname"
                            value={this.state.surname}
                            onChange={this.handleInputChange}
                        />
                    </div><br />

                    <div>
                        <label>Email:</label><br />
                        <input
                            type="email"
                            name="email"
                            value={this.state.email}
                            onChange={this.handleInputChange}
                        />
                    </div><br />

                    <div>
                        <label>Phone number:</label><br />
                        <input
                            type="tel"
                            name="phone"
                            value={this.state.phone}
                            onChange={this.handleInputChange}
                        />
                    </div><br />

                    <div>
                        <label>Referral code:</label><br />
                        <input
                            type="text"
                            name="referal_code"
                            value={this.state.referal_code}
                            onChange={this.handleInputChange}
                        />
                    </div><br />

                    <div>
                        <button type="submit">Confirm edit</button>
                    </div>
                </form>
            </div>
        );
    }
}
