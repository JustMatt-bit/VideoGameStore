import React, { Component } from 'react';
import './NavMenu.css';

export class LoyaltyProgram extends Component {
    static displayName = LoyaltyProgram.name;

    constructor(props) {
        super(props);
        this.state = {
            isLoading: true,
            loyaltyTier: null,
            nextTier: null,
            userProgress: 0,
            username: ''
        };
    }

    componentDidMount() {
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (authCookie) {
            const username = authCookie.split('=')[1];
            this.setState({ username }, this.fetchUserLoyaltyData);
        } else {
            this.setState({ isLoading: false });
        }
    }

    fetchUserLoyaltyData = () => {
        const { username } = this.state;
        fetch(`/api/loyalty/GetUserTierDetails/${username}`)
            .then(response => response.json())
            .then(data => {
                console.log('API Response Data:', data); // Add this line for debugging
                this.setState({
                    isLoading: false,
                    loyaltyTier: data.currentTier,
                    nextTier: data.nextTier,
                    userProgress: data.userProgress,
                });
            })
            .catch(error => {
                console.error('Error fetching loyalty data:', error);
                this.setState({ isLoading: false });
            });
    }


    renderLoyaltyInfo = () => {
        const { loyaltyTier, nextTier, userProgress } = this.state;

        if (loyaltyTier) {
            return (
                <div>
                    <h3>Your current loyalty tier: {loyaltyTier.name}</h3>
                    <p>{loyaltyTier.description}</p>
                    <p>Discount coefficient: {loyaltyTier.discountCoefficient.toFixed(2)}</p>
                    <p>You have accumulated {userProgress} loyalty points.</p>
                    {nextTier ? (
                        <p>You are {loyaltyTier.pointsTo - userProgress} points away from {nextTier.name}.</p>
                    ) : (
                        <p>You are at the max tier, congratulations!</p>
                    )}
                </div>
            );
        }
    }

    render() {
        const { isLoading, loyaltyTier } = this.state;

        return (
            <div>
                {isLoading ? (
                    <p>Loading...</p>
                ) : (
                    loyaltyTier ? this.renderLoyaltyInfo() : <p>No loyalty data available.</p>
                )}
            </div>
        );
    }

}
