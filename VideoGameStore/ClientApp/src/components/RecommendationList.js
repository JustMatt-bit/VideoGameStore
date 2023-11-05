import React, { Component, Button } from 'react';
import gameImage from "../assets/liesofp.png";
import { useNavigate } from "react-router-dom";



export class RecommendationList extends Component {

    render() {

        return (

            <div>
                <h1 id="tabelLabel" >Rekomendacijos jums</h1>
                <div style={{ margin: ' 20px auto', width: '18%' }}>
                    <a style={{ color: 'black', textDecoration: 'none' }} href="/fetch-products">
                        <div style={{ border: '1px black solid' }}>
                            <img style={{ height: '300px', width: '100%', margin: ' auto' }} src={gameImage} />
                            <div style={{ width: '50%', margin: ' auto', textAlign: 'center' }}>
                                <h6 >Lies of P</h6>
                                <p>Kaina: 60eur</p>
                            </div>
                        </div>
                    </a>
                </div>


                <div style={{ margin: ' 20px auto', width: '18%' }}>
                    <a style={{ color: 'black', textDecoration: 'none' }} href="/fetch-products">
                        <div style={{ border: '1px black solid' }}>
                            <img style={{ height: '300px', width: '100%', margin: ' auto' }} src={gameImage} />
                            <div style={{ width: '50%', margin: ' auto', textAlign: 'center' }}>
                                <h6 >Lies of P</h6>
                                <p>Kaina: 60eur</p>
                            </div>
                        </div>
                    </a>
                </div>


                <div style={{ margin: ' 20px auto', width: '18%' }}>
                    <a style={{ color: 'black', textDecoration: 'none' }} href="/fetch-products">
                        <div style={{ border: '1px black solid' }}>
                            <img style={{ height: '300px', width: '100%', margin: ' auto' }} src={gameImage} />
                            <div style={{ width: '50%', margin: ' auto', textAlign: 'center' }}>
                                <h6 >Lies of P</h6>
                                <p>Kaina: 60eur</p>
                            </div>
                        </div>
                    </a>
                </div>

            </div>

        );
    }
}