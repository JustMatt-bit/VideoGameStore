﻿import React, { Component, useState } from 'react';
import "./FetchFeedback.css";


const StarRating = () => {

    // NENAUDOTI USE STATES
    const [rating, setRating] = useState(0);
    const [hover, setHover] = useState(0);
    return (
        <div className="star-rating">
            {[...Array(5)].map((star, index) => {
                index += 1;
                return (
                    <button
                        type="button"
                        key={index}
                        className={index <= (hover || rating) ? "on" : "off"}
                        onClick={() => setRating(index)}
                        onMouseEnter={() => setHover(index)}
                        onMouseLeave={() => setHover(rating)}
                    >
                        <span className="star">&#9733;</span>
                    </button>
                );
            })}
        </div>
    );
};

export class FetchFeedback extends Component {
    //static displayName = FetchFeedback.name;

    constructor(props) {
        super(props);
        this.state = { feedback: [], loading: true, newFeedbackText: "" };

        // Bind functions
        this.handleInputChange = this.handleInputChange.bind(this);
        this.submitFeedback = this.submitFeedback.bind(this);
    }

    handleInputChange(event) {
        this.setState({ newFeedbackText: event.target.value });
    }


    componentDidMount() {
        const searchParams = new URLSearchParams(window.location.search);
        const productId = searchParams.get('id') || null; // Default to 1 if not present
        if (productId != null)
            this.populateVideoGameData(productId);
    }

    async submitFeedback() {
        const { newFeedbackText } = this.state;
        const searchParams = new URLSearchParams(window.location.search);
        const productId = searchParams.get('id') || null;
        const authCookie = document.cookie.split('; ').find(row => row.startsWith('AuthCookie'));
        const username = authCookie ? authCookie.split('=')[1] : null;

        if (productId && newFeedbackText && username) {
            const feedback = {
                text: newFeedbackText,
                // Add other required fields, set default values if needed
                date: new Date().toISOString(),
                rating: 0, // example default value
                rating_count: 0, // example default value
                is_flagged: false, // example default value
            };

            // Call the API to submit the feedback
            try {
                const response = await fetch(`api/feedback/${productId}`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ feedback, username }) // Ensure proper JSON structure
                });

                if (response.ok) {
                    this.setState({ newFeedbackText: "" });
                    this.populateVideoGameData(productId);
                } else {
                    // Handle non-OK responses
                    const errorData = await response.json();
                    console.error('Error submitting feedback:', errorData);
                }
            } catch (error) {
                console.error('Error submitting feedback:', error);
            }
        }
    }


    static renderFeedbackCards(feedback) {
        return (
            feedback.map(feedbackItem => (
                <div className="feedback-card" key={feedbackItem.id}>
                    <div className="feedback-text">{feedbackItem.text}</div>
                    <div className="feedback-info">
                        <div>Date: {feedbackItem.date.split("T")[0]}</div>
                        {/*<div>Rating: {feedbackItem.rating}</div>*/}
                        {/*<div>Rating Count: {feedbackItem.rating_count}</div>*/}
                        {/*<div>Flagged: {feedbackItem.is_flagged ? "Yes" : "No"}</div>*/}
                        <div>User: {feedbackItem.account_name}</div>
                        <StarRating />
                    </div>
                </div>
            ))
        );
    }

    render() {
        const { feedback, loading, newFeedbackText } = this.state;
        let contents = loading
            ? <p><em>Loading...</em></p>
            : FetchFeedback.renderFeedbackCards(feedback);

        return (
            <div>
                <br></br>
                <div className="feedback-container">
                    {contents}
                    <div className="feedback-input">
                        <textarea
                            className="feedbackTextArea"
                            value={newFeedbackText}
                            onChange={this.handleInputChange}
                            placeholder="Write your feedback here..."
                            maxLength={500}
                            minLength={3}
                        />
                        <br></br>
                        <button onClick={this.submitFeedback}>Submit Feedback</button>
                    </div>
                </div>
            </div>
        );
    }

    async populateVideoGameData(productId) {
        //const productId = this.props.productId;
        //const productId = 1;
        const response = await fetch(`api/feedback/${productId}`);
        const data = await response.json();
        this.setState({ feedback: data, loading: false });
    }
}