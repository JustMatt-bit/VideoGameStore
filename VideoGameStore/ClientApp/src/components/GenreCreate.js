import React, { Component } from 'react';
import { Routes, Route, useNavigate, Navigate, Link, createSearchParams } from 'react-router-dom';


export class GenreCreate extends Component {
    static displayName = GenreCreate.name;

    constructor(props) {
        super(props);
        this.state = { genreName: '', genreDescription: '', error: [] };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;
        this.setState({
            [name]: value
        });
    }

    sanitizeString(stringValue) {
        stringValue = stringValue.trim();
        //stringValue = encodeURI(stringValue);
        return stringValue;
    }

    async handleSubmit(event) {
        event.preventDefault();
        this.setState({ error: [] });
        let { genreName, genreDescription } = this.state;
        genreName = this.sanitizeString(genreName);
        genreDescription = this.sanitizeString(genreDescription);
        
        let errors = [];
        //Perform validation
        if (!genreName) {
            errors.push("*Įveskite žanro pavadinimą");
        } else if (genreName.length > 30) {
            errors.push("*Žanro pavadinimas negali viršyti 30 simbolių");
        } else {
            const response = await fetch('api/products/GenreExists', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(genreName),
            });
            const data = await response.json();
            if (data) {
                errors.push("*Žanras jau egzistuoja");
            } else {
                errors.push("");
            }
        }
        if (!genreDescription) {
            errors.push("*Įveskite žanro aprašą");
        } else if (genreDescription.length > 1000) {
            errors.push("*Žanro pavadinimas negali viršyti 1000 simbolių");
        }
        if (errors.filter(n => n != '').length == 0) {
            let name = genreName;
            let description = genreDescription;
            const response = await fetch('api/products/CreateGenre', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ name, description }),
            });
            const data = await response.json();
            if (response.ok) {
                window.alert("Sėkmingai sukūrėte žanrą");
                window.location.href = '/product-control';
            }
        } else {
            this.setState({ error : errors })
        }
    }

    render() {
        let contents = <></>;
        const { error } = this.state;
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (!authCookie) {
            contents = <Navigate to="/fetch-products" replace={true} />
        }
        return (
            <div>
                {contents }
                <form onSubmit={this.handleSubmit}>
                    <div>
                        <div>
                            <label>
                                Žanras:
                                <input type="text" name="genreName" value={this.state.genreName} onChange={this.handleChange} />
                            </label>
                            {error[0] && <div style={{ color: 'red' }}>{error[0]}</div>}
                        </div>
                        <br />
                        <div>
                            <label>
                                Aprašas:
                                <textarea name="genreDescription" value={this.state.genreDescription} onChange={this.handleChange} />
                            </label>
                            {error[1] && <div style={{ color: 'red' }}>{error[1]}</div>}
                        </div>
                        <br />
                        <input type="submit" value="Kurti" />

                    </div>
                </form>

            </div>
        );
    }
}
