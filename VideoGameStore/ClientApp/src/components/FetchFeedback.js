import React, { Component, useState } from 'react';
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
        this.state = {
            feedback: [],
            loading: true,
            newFeedbackText: "",
            username: null,
            userType: null,
            replyText: "",
            replyToId: null,
            showFeedbackInput: false,
        };

        // Bind functions
        this.handleInputChange = this.handleInputChange.bind(this);
        this.submitFeedback = this.submitFeedback.bind(this);
        this.handleReplyChange = this.handleReplyChange.bind(this);
        this.submitReply = this.submitReply.bind(this);
        this.toggleFeedbackInput = this.toggleFeedbackInput.bind(this); // New function binding

    }

    toggleFeedbackInput() {
        this.setState(prevState => ({
            showFeedbackInput: !prevState.showFeedbackInput
        }));
    }

    handleInputChange(event) {
        this.setState({ newFeedbackText: event.target.value });
    }

    handleReplyChange(event) {
        this.setState({ replyText: event.target.value });
    }

    async submitReply(replyingToId, replyText) {
        const searchParams = new URLSearchParams(window.location.search);
        const productId = searchParams.get('id') || null;
        const authCookie = document.cookie.split('; ').find(row => row.startsWith('AuthCookie'));
        const username = authCookie ? authCookie.split('=')[1] : null;

        if (productId && replyText && username && replyingToId != null) {
            const feedback = {
                text: replyText,
                date: new Date().toISOString(),
                rating: 0, // example default value
                rating_count: 0, // example default value
                is_flagged: false, // example default value
                replying_to_id: replyingToId // replying to this feedback id
            };

            try {
                const response = await fetch(`api/feedback/${productId}`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ feedback, username }) // Ensure proper JSON structure
                });

                if (response.ok) {
                    this.setState({ replyText: "", replyToId: null });
                    this.populateVideoGameData(productId); // Refresh feedback list
                } else {
                    const errorData = await response.json();
                    console.error('Error submitting reply:', errorData);
                }
            } catch (error) {
                console.error('Error submitting reply:', error);
            }
        } else {
            console.error('Missing information for submitting reply');
        }
    }


    componentDidMount() {
        const searchParams = new URLSearchParams(window.location.search);
        const productId = searchParams.get('id') || null;
        if (productId != null)
            this.populateVideoGameData(productId);

        const authCookie = document.cookie.split('; ').find(row => row.startsWith('AuthCookie'));
        if (authCookie) {
            const username = authCookie.split('=')[1];
            this.setState({ username });

            // Fetch user details to get userType
            fetch(`/api/user/GetUserDetails/${username}`)
                .then(response => response.json())
                .then(data => {
                    this.setState({
                        userType: data.fk_user_type // Set userType based on API response
                    });
                })
                .catch(error => {
                    console.error('Error fetching user details:', error);
                });
        }
    }

    async fetchUserType() {
        const authCookie = document.cookie.split('; ').find(row => row.startsWith('AuthCookie'));
        const username = authCookie ? authCookie.split('=')[1] : null;
        if (username) {
            try {
                const response = await fetch(`/api/user/GetType/${username}`);
                if (response.ok) {
                    const data = await response.json();
                    this.setState({ userType: data.userType });
                }
            } catch (error) {
                console.error('Error fetching user type:', error);
            }
        }
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


    renderFeedbackCards(feedback) {
        const { userType, replyToId, replyText } = this.state;

        // Function to render individual feedback or reply
        const renderFeedbackOrReply = (item, isReply = false) => (
            <div className={`feedback-card ${isReply ? "reply-card" : ""}`} key={item.id}>
                <div className="feedback-text">{item.text}</div>
                <div className="feedback-info">
                    <div>Date: {item.date.split("T")[0]}</div>
                    <div>User: {item.account_name}</div>
                    {!isReply &&(
                        <StarRating />
                    )}
                </div>
                {userType === 2 && !isReply && (
                    <div>
                        <button onClick={() => this.setState({ replyToId: item.id })}>Reply</button>
                        {replyToId === item.id && (
                            <div>
                                <textarea value={replyText} onChange={this.handleReplyChange} />
                                <button onClick={() => this.submitReply(item.id, replyText)}>Submit Reply</button>
                            </div>
                        )}
                    </div>
                )}
            </div>
        );

        return (
            feedback.map(feedbackItem => (
                <div key={feedbackItem.id}>
                    {renderFeedbackOrReply(feedbackItem)}
                    {feedbackItem.replies && feedbackItem.replies.map(reply => renderFeedbackOrReply(reply, true))}
                </div>
            ))
        );
    }





    render() {
        const { feedback, loading, newFeedbackText, username, userType, showFeedbackInput } = this.state;

        let contents = loading
            ? <p><em>Loading...</em></p>
            : this.renderFeedbackCards(feedback);

        let feedbackInputSection = showFeedbackInput && (
            <div className="feedback-input">
                <textarea
                    className="feedbackTextArea"
                    value={newFeedbackText}
                    onChange={this.handleInputChange}
                    placeholder="Write your feedback here..."
                    maxLength={500}
                    minLength={3}
                />
                <br />
                <button onClick={this.submitFeedback}>Submit Feedback</button>
            </div>
        );

        return (
            <div>
                <div>
                    <br />
                    <h1>User feedback:</h1>
                    <div className="feedback-container">
                        {contents}
                        {username && userType === 1 && (
                            <button onClick={this.toggleFeedbackInput}>
                                {showFeedbackInput ? 'Cancel' : 'Create Feedback'}
                            </button>
                        )}
                        {feedbackInputSection}
                    </div>
                    </div>
                <br></br>
                <br></br>
                <br></br>
            </div>

        );
    }

    organizeFeedback(feedbackList) {
        const feedbackMap = {};

        // First, map all feedback by their ID
        feedbackList.forEach(feedback => {
            feedbackMap[feedback.id] = { ...feedback, replies: [] };
        });

        // Then, associate replies with their corresponding feedback
        feedbackList.forEach(feedback => {
            if (feedback.replying_to_id != null) {
                feedbackMap[feedback.replying_to_id].replies.push(feedback);
            }
        });

        // Finally, filter out the replies from the top level, as they are now nested
        return Object.values(feedbackMap).filter(feedback => feedback.replying_to_id == null);
    }

    async populateVideoGameData(productId) {
        //const productId = this.props.productId;
        //const productId = 1;
        const response = await fetch(`api/feedback/${productId}`);
        const data = await response.json();
        const organizedFeedback = this.organizeFeedback(data);
        this.setState({ feedback: organizedFeedback, loading: false });
    }
}