import React, { Component } from 'react';


export class RecommendationList extends Component {
    static displayName = RecommendationList.name;
    render() {

        return (

            <div>
                <h1 id="tabelLabel" >Your recommendations</h1>
                <div style={{ margin: ' 20px auto', width: '18%' }}>
                    <a style={{ color: 'black', textDecoration: 'none' }} href="/product">
                        <div style={{ border: '1px black solid' }}>
                            <img style={{ height: '300px', width: '100%', margin: ' auto' }} src="/images/liesofp.png" />
                            <div style={{ width: '50%', margin: ' auto', textAlign: 'center' }}>
                                <h6 >Lies of P</h6>
                                <p>Price: 60eur</p>
                            </div>
                        </div>
                    </a>
                </div>


                <div style={{ margin: ' 20px auto', width: '18%' }}>
                    <a style={{ color: 'black', textDecoration: 'none' }} href="/product">
                        <div style={{ border: '1px black solid' }}>
                            <img style={{ height: '300px', width: '100%', margin: ' auto' }} src="/images/liesofp.png" />
                            <div style={{ width: '50%', margin: ' auto', textAlign: 'center' }}>
                                <h6 >Lies of P</h6>
                                <p>Price: 60eur</p>
                            </div>
                        </div>
                    </a>
                </div>


                <div style={{ margin: ' 20px auto', width: '18%' }}>
                    <a style={{ color: 'black', textDecoration: 'none' }} href="/product">
                        <div style={{ border: '1px black solid' }}>
                            <img style={{ height: '300px', width: '100%', margin: ' auto' }} src="/images/liesofp.png" />
                            <div style={{ width: '50%', margin: ' auto', textAlign: 'center' }}>
                                <h6 >Lies of P</h6>
                                <p>Price: 60eur</p>
                            </div>
                        </div>
                    </a>
                </div>

            </div>

        );
    }
}