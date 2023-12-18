import React, { Component } from 'react';

export class Verify extends Component {
    static displayName = Verify.name;

    componentDidMount() {
        // Get username and token from the URL
        const urlParams = new URLSearchParams(window.location.search);
        const username = urlParams.get('username');
        const token = urlParams.get('token');
       

        // Check if username and token are present
        if (username && token) {
            // Make API call to verify the account
            this.verifyAccount(username, token);
        } else {
            // Display an error message if username or token is missing
            alert('Invalid verification link. Please check the link and try again.');
        }
    }
    sendWelcomeEmail(username) {
        // Make a request to your backend to send the welcome email
        fetch(`/api/email/sendWelcomeEmail/${username}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Error sending welcome email');
                }
                return response.json();
            })
            .then(data => {
                console.log(data.message);
            })
            .catch(error => {
                console.error('Error sending welcome email:', error);
            });
    }
    async verifyAccount(username, token) {
        try {
            // Make API call to verify the account
            //console.log(token);
            //console.log(username);
            const response = await fetch(`/api/user/VerifyAccount/${username}/${token}`);

            if (response.ok) {
                // Check if the response status is OK (200)
                const data = await response.json();
                this.sendWelcomeEmail(username);
                console.log('Account verified successfully!');
                setTimeout(() => {
                    window.location.href = '/fetch-account';
                }, 3000); // Redirect after 3 seconds
            } else {
                // Log an error message if the response status is not OK
                console.error('Error verifying account. Please try again.');
            }
        } catch (error) {
            // Display error message or handle the error
            console.error('Error verifying account. Please try again.', error);
        }
    }

    render() {
        return (
            <div>
                <h1>Account Verification</h1>
                <p>Verifying your account...</p>
            </div>
        );
    }
}

