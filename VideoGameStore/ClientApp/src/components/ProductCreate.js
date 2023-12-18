import React, { Component } from 'react';
import { Routes, Route, useNavigate, Navigate, Link, createSearchParams } from 'react-router-dom';
import Multiselect from 'multiselect-react-dropdown';

export class ProductCreate extends Component {
    static displayName = ProductCreate.name;

    constructor(props) {
        super(props);
        this.state = { gameTypes: [], genres: [], selectedGenres: [], developers: [], error: [], name: '', price: '', stock: '', description: '', releaseDate: '', beingSold: 1, gameType: 1, developerID: '', developerName: '', developerCountry: '', image: null };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.onSelectGenre = this.onSelectGenre.bind(this);
        this.onRemoveGenre = this.onRemoveGenre.bind(this);
        this.onSelectDeveloper = this.onSelectDeveloper.bind(this);
        this.onRemoveDeveloper = this.onRemoveDeveloper.bind(this);
        
    }

    componentDidMount() {
        this.populateData();
    }

    async populateData() {
        const response1 = await fetch('api/products/GetGameTypes', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
        });
        const gameTypes = await response1.json();
        const response2 = await fetch('api/products/GetGenres', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
        });
        const genres = await response2.json();
        const response3 = await fetch('api/products/GetDevelopers', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
        });
        const developers = await response3.json();
        this.setState({ gameTypes: gameTypes, genres: genres, developers: developers });

    }
   

    onSelectGenre(selectedList, selectedItem) {
        let selectedGenres = this.state.selectedGenres;
        selectedGenres.push(selectedItem);
        this.setState({ selectedGenres: selectedGenres });
    }

    onRemoveGenre(selectedList, removedItem) {
        let selectedGenres = this.state.selectedGenres;
        selectedGenres = selectedGenres.filter(n => n.id != removedItem.id);
        this.setState({ selectedGenres: selectedGenres });
    }

    onSelectDeveloper(selectedList, selectedItem) {
        this.setState({ developerID: selectedItem.id });
    }

    onRemoveDeveloper(selectedList, removedItem) {
        this.setState({ developerID: null });
    }


    handleChange(event) {
        if (event.target.type === 'file') {
            event.preventDefault();
            let form = new FormData();
            for (var index = 0; index < event.target.files.length; index++) {
                var element = event.target.files[index];
                form.append('image', element);
            }
            this.setState({ file: form });
        }
        
        
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
        let { name, price, stock, description, releaseDate, beingSold, gameType, developerID, developerName, developerCountry, selectedGenres, image } = this.state;
        let errors = [];
        //Perform validation
        name = this.sanitizeString(name);
        if (!name) {
            errors.push("*Product name field can't be empty");
        } else if (name.length > 100) {
            errors.push("*Product name can't be longer than 100 symbols");
        } else {
            errors.push("");
        }
        description = this.sanitizeString(description);
        if (!description) {
            errors.push("*Description field can't be empty");
        } else if (description.length > 1000) {
            errors.push("*Description can't be longer than 1000 symbols");
        } else {
            errors.push("");
        }
        if (!releaseDate) {
            errors.push("*Release date must be selected");
        } else {
            errors.push("");
        }
        if (selectedGenres.length == 0) {
            errors.push("*Select at least one genre");
        } else {
            errors.push("");
        }
        stock = this.sanitizeString(stock);
        if (!stock) {
            errors.push("*Stock must be entered");
        } else if (!Number.isInteger(Number(stock)) || stock < 0) {
            errors.push("*Stock must be a positive integer");
        } else if (beingSold && stock == 0) {
            errors.push("*Stock must be non zero if product is sellable");
        }
        else {
            errors.push("");
        }
        price = this.sanitizeString(price);
        if (!price) {
            errors.push("*Price must be entered");
        } else if (!Number(price) || price <= 0) {
            errors.push("*Price must be a positive number");
        } else {
            errors.push("");
        }
        

        const selectedFile = document.getElementById("file").files[0];
        //this.setState({ image: selectedFile });
        if (!image) {
            errors.push("*Image must be selected");
        } else {
            let imageExtension = image.split('.');
            if (imageExtension.lenght == 1) {
                errors.push("*Must be a jpg, png or jpeg file");
            } else {
                imageExtension = imageExtension[imageExtension.length - 1];
                if (imageExtension == "jpg" || imageExtension == "png" || imageExtension == "jpeg") {
                    errors.push("");
                } else {
                    errors.push("*Must be a jpg, png or jpeg file");
                }
            }
        }

        if (!developerID) {
            developerCountry = this.sanitizeString(developerCountry);
            developerName = this.sanitizeString(developerName);
            if (!developerCountry && !developerName) {
                errors.push("* Create new developer or select existing one");
            } else {
                if (!developerCountry && developerName) {
                    errors.push("");
                    if (developerName.length > 30) {
                        errors.push("* Developer name can't be longer than 30 symbols");
                    } else {
                        errors.push("");
                    }
                    errors.push("* Developer country is needed");

                } else if (developerCountry && !developerName) {
                    errors.push("");
                    errors.push("* Developer name is needed");
                    if (developerCountry.length > 30) {
                        errors.push("* Developer country can't be longer than 30 symbols");
                    }
                }
            }
        } else {
            errors.push("");
        }
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));
        const authCookieValue = authCookie.split('=')[1];
        const username = authCookieValue;
        if (errors.filter(n => n != '').length == 0) {
            let response = 0;
            let data = 0;
            let developerId = developerID;
            //create developer if new
            if (!developerID) {
                response = await fetch('api/products/CreateDeveloper', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({ name: developerName, country: developerCountry }),
                });
                data = await response.json();
                developerId = data;
                
            }
            
            //create product
            response = await fetch('api/products/CreateProduct', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    name: name,
                    price: price,
                    stock: stock,
                    description: description,
                    release_date: releaseDate,
                    being_sold: beingSold==1?true:false,
                    fk_game_type: gameType,
                    fk_developer: developerId,
                    fk_account: username,
                    image: ""
                }),
            });
            data = await response.json();
            let productId = data;
            let fileData = this.state.file;
            fileData.append("id", data);
            this.setState({ file: fileData });
            // upload image
            response = await fetch('api/products/UploadImage', {
                method: 'POST',
                mode: 'cors',
                headers: {
                    'Accept': 'application/json',
                },
                body: this.state.file,
            });
            data = await response.json();
            //create relation between product and genres
            response = await fetch('api/products/GenresProductConnection', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    id: productId,
                    genres: selectedGenres
                }),
            });
            data = await response.json();
            if (response.ok) {
                window.alert("Product creation succeded");
                window.location.href = '/product-control';
            }
        } else {
            this.setState({ error: errors })
        }
    }

    render() {
        let contents = <></>;
        console.log(this.state);
        const { error } = this.state;
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (!authCookie) {
            contents = <Navigate to="/fetch-products" replace={true} />
        }
        return (

            <div style={{ marginBottom: '10%', textAlign: 'center' }} id="prodCreation">
                <h2>Product creation</h2>
                <form onSubmit={this.handleSubmit}>
                    <div>
                        <label>Name: </label><br />
                        <input type="text" name="name" value={this.state.name} onChange={this.handleChange} /><br />
                        {error[0] && <div style={{ color: 'red' }}>{error[0]}</div>}
                        <label>Description: </label><br />
                        <input type="text" name="description" value={this.state.description} onChange={this.handleChange} /><br />
                        {error[1] && <div style={{ color: 'red' }}>{error[1]}</div>}
                        <label>Release date: </label><br />
                        <input type="date" name="releaseDate" value={this.state.releaseDate} onChange={this.handleChange} /><br />
                        {error[2] && <div style={{ color: 'red' }}>{error[2]}</div>}
                        <label>Genres: </label><br />
                        <Multiselect style={{ searchBox: { width: "50%", margin: "auto" } }}
                            options={this.state.genres}
                            displayValue="name"
                            onSelect={this.onSelectGenre}
                            onRemove={this.onRemoveGenre}
                            showCheckbox
                            avoidHighlightFirstOption
                            />
                        {error[3] && <div style={{ color: 'red' }}>{error[3]}</div>}
                        <label>Type:</label><br />
                        {this.state.gameType == 1 ? <> <input type="radio" id="gameTypePhy" name="gameType" value="1" checked onChange={this.handleChange} /></> : <> <input type="radio" id="gameTypePhy" name="gameType" value="1" onChange={this.handleChange} /></>}
                        <label for="gameTypePhy" style={{ marginRight: '10px' }}>Physical copy</label>
                        {this.state.gameType == 2 ? <><input type="radio" id="gameTypePhy" name="gameType" value="2" checked onChange={this.handleChange} /></> : <><input type="radio" id="gameTypeKey" name="gameType" value="2" onChange={this.handleChange} /></>}
                        <label for="gameTypeKey">Key code</label>
                        <br />
                        <label>Stock: </label><br />
                        <input type="text" name="stock" value={this.state.stock} onChange={this.handleChange} /><br />
                        {error[4] && <div style={{ color: 'red' }}>{error[4]}</div>}
                        <label>Price: </label><br />
                        <input type="text" name="price" value={this.state.price} onChange={this.handleChange} /><br />
                        {error[5] && <div style={{ color: 'red' }}>{error[5]}</div>}
                        <label>Is sellable: </label><br />
                        {this.state.beingSold == 1 ? <><input type="radio" id="sellable" name="beingSold" value="1" checked onChange={this.handleChange} /></> : <><input type="radio" id="sellable" name="beingSold" value="1" onChange={this.handleChange} /></>}
                        <label for="sellable" style={{ marginRight: '10px' }}>Yes</label>
                        {this.state.beingSold == 0 ? <><input type="radio" id="notSellable" name="beingSold" value="0" checked onChange={this.handleChange} /></> : <><input type="radio" id="notSellable" name="beingSold" value="0" onChange={this.handleChange} /></>}
                        <label for="notSellable">No</label><br />
                        <label for="file" style={{ marginBottom: '10px' }}>Image (jpg, png, jpeg) :</label><br />
                        <input id="file" type="file" name="image" value={this.state.image} onChange={this.handleChange} /><br />
                        {error[6] && <div style={{ color: 'red' }}>{error[6]}</div>}
                    </div>
                    <div style={{ flexDirection: 'row',width: '100%', marginTop: '30px'  }}>
                        <h2 style={{ textAlign: 'center' }}>Developer</h2>
                        <div style={{ display: 'inline-block', width: '50%', textAlign: 'center'}}>
                            <h5>Create developer</h5>
                            <label>Developer name:</label><br />
                            {this.state.developerID ? <><input type="text" disabled /><br /></> : <><input type="text" name="developerName" value={this.state.developerName} onChange={this.handleChange} /><br />{error[8] && <div style={{ color: 'red' }}>{error[8]}</div>}</>}
                            <label>Country:</label><br />
                            {this.state.developerID ? <><input type="text" disabled /><br /></> : <><input type="text" name="developerCountry" value={this.state.developerCountry} onChange={this.handleChange} /><br />{error[9] && <div style={{ color: 'red' }}>{error[9]}</div>}</>}

                        </div>
                        <div style={{ display: 'inline-block', width: '50%', textAlign: 'center', alignSelf:'center'}}>
                            
                            <h5>Select developer</h5>
                            <Multiselect 
                                options={this.state.developers}
                                displayValue="name"
                                onSelect={this.onSelectDeveloper}
                                onRemove={this.onRemoveDeveloper}
                                selectionLimit="1"
                                closeOnSelect="false"
                                disable={this.state.developerName ? true : (this.state.developerCountry ? true : false)} avoidHighlightFirstOption
                            />
                            
                            </div>
                    </div>
                    {error[7] && <div style={{ color: 'red' }}>{error[7]}</div>}
                    <div style={{ textAlign: 'center', marginTop:'20px' }}>
                        <button type="submit">Create</button>
                    </div>
                </form>

            </div>

        );
    }



}