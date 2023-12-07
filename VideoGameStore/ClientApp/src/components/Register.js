import React, { Component } from 'react';
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
            date: new Date().toLocaleDateString()
        };
    }

    handleInputChange = (event) => {
        const { name, value } = event.target;
        this.setState({ [name]: value });
    };

    handleSubmit = (event) => {
        event.preventDefault();
        console.log(this.state)
        // Send registration data to the server
        fetch('api/user', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(this.state)
        })
            .then((response) => {
                // Log the response status
                console.log('Response Status:', response.status);

                // Check if the response status is OK (200)
                if (response.ok) {
                     return response.json(); // Parse JSON if the response is OK
                } else {
                    throw new Error('Network response was not ok.');
                }
            })
            .then((data) => {
                console.log('Registration response:', data);
                // Handle success or failure
            })
            .catch((error) => {
                console.error('Registration failed:', error);
            });
    };
    

    render() {
        return (
            <div>
                <h2>Registration</h2>
                <form onSubmit={this.handleSubmit}>
                        <div>
                            <label>Username:</label><br />
                            <input
                                type="text"
                                name="username"
                                value={this.state.username}
                                onChange={this.handleInputChange}
                            />
                        </div><br />

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
export default Register;
