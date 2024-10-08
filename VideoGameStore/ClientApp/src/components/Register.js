﻿import React, { Component } from 'react';
import './NavMenu.css';

export class Register extends Component {
    static displayName = Register.name;

    constructor(props) {
        super(props);

        this.state = {
            username: '',
            password: '',
            name: '',
            surname: '',
            email: '',
            phone: '',
            referal_code: '',
            date: new Date().toLocaleDateString(),
            error: null,
            registrationStatus: null,
        };
    }
    updateLoginStatus = (isLoggedIn, username) => {
        this.setState({ isLoggedIn, username });
    };
    handleInputChange = (event) => {
        const { name, value } = event.target;
        this.setState({ [name]: value, error: null });
    };

    handleSubmit = (event) => {
        event.preventDefault();

        // Perform basic validation
        const { username, password, name, surname, email, phone, referal_code } = this.state;
        if (!username || !password || !name || !surname || !email || !phone) {
            this.setState({ error: 'All fields are required.', registrationStatus: null });
            return;
        }

        // Check if referral code exists (if provided)
        if (referal_code) {
            this.checkReferralCode(referal_code);
        } else {
            this.registerUser(); // Proceed with registration if no referral code is provided
        }
    };

    checkReferralCode = (referal_code) => {
        fetch(`api/referral/CheckReferralCode/${referal_code}`)
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    throw new Error('Referral code not found.');
                }
            })
            .then(data => {
                if (data.exists) {
                    this.registerUser(); // Referral code exists, proceed with registration
                } else {
                    throw new Error('No such referral code.');
                }
            })
            .catch(error => {
                console.error('Referral code check failed:', error);
                this.setState({ error: error.message, registrationStatus: null });
            });
    };

    registerUser = () => {
        // Send registration data to the server
        fetch('api/user/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(this.state),
        })
            .then((response) => {
                if (response.ok) {
                    return response.json();
                } else {
                    throw new Error('Network response was not ok.');
                }
            })
            .then((data) => {
                this.setState({ registrationStatus: 'Registration successful!', error: null });
                setTimeout(() => {
                    window.location.href = '/fetch-account';
                }, 3000); // Redirect after 3 seconds
            })
            .catch((error) => {
                console.error('Registration failed:', error);
                this.setState({ registrationStatus: null, error: 'Registration failed. Please try again.' });
            });
    };

    render() {
        const { error, registrationStatus } = this.state;

        return (
            <div>
                <h2>Registration</h2>
                {error && <div style={{ color: 'red' }}>{error}</div>}
                {registrationStatus && <div style={{ color: 'green' }}>{registrationStatus}</div>}
                <form onSubmit={this.handleSubmit}>
                        <div>
                        <label><subscript>*</subscript>Username:</label><br />
                            <input
                                type="text"
                                name="username"
                                value={this.state.username}
                                onChange={this.handleInputChange}
                            />
                        </div><br />

                        <div>
                        <label><subscript>*</subscript>Password:</label><br />
                            <input
                                type="password"
                                name="password"
                                value={this.state.password}
                                onChange={this.handleInputChange}
                            />
                        </div><br />

                        <div>
                        <label><subscript>*</subscript>Name:</label><br />
                            <input
                                type="text"
                                name="name"
                                value={this.state.name}
                                onChange={this.handleInputChange}
                            />
                        </div><br />

                        <div>
                        <label><subscript>*</subscript>Surname:</label><br />
                            <input
                                type="text"
                                name="surname"
                                value={this.state.surname}
                                onChange={this.handleInputChange}
                            />
                        </div><br />

                        <div>
                        <label><subscript>*</subscript>Email:</label><br />
                            <input
                                type="email"
                                name="email"
                                value={this.state.email}
                                onChange={this.handleInputChange}
                            />
                        </div><br />

                        <div>
                        <label><subscript>*</subscript>Phone number:</label><br />
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
                                value={this.state.referal}
                                onChange={this.handleInputChange}
                            />
                        </div><br />

                    <div>
                        <button type="submit">Register</button>
                    </div>
                </form>
            </div>
        );
    }
}
