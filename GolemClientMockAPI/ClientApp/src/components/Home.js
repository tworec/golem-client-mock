import React, { Component } from 'react';
import { Node } from './Node';


export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { stats: null, loading: true };

        this.refresh();
    }

    componentDidMount() {
        this.interval = setInterval(() => this.refresh(), 1000);
    }
    componentWillUnmount() {
        clearInterval(this.interval);
    }

    refresh() {
        fetch('admin/marketStats')
            .then(response => response.json())
            .then(data => {
                this.setState({ stats: data, loading: false });
            });

    }


    renderNodeStatsTable(nodeStatsList) {
        //return (
        //    <table className='table'>
        //        <thead>
        //            <tr>
        //                <th>NodeId</th>
        //                <th>Connected</th>
        //                <th>Subscriptions</th>
        //                <th>Last Active</th>
        //            </tr>
        //        </thead>
        //        <tbody>
        //            {nodeStatsList.map(node =>
        //                <Node key={node.nodeId} node={node} />
        //            )}
        //        </tbody>
        //    </table>

        //    );

        return (
            <div className='container'>
                <div className='row' style={{ fontWeight: 'bolder', borderTop: '1px solid', borderBottom: '1px solid' }}>
                    <div className='col-sm-3'>NodeId</div>
                    <div className='col-sm-3'>Connected</div>
                    <div className='col-sm-2'>Subscriptions</div>
                    <div className='col-sm-3'>Last Active</div>
                </div>
                {nodeStatsList.map(node =>
                    <Node key={node.nodeId} node={node} />
                )}
            </div>
        );
    }


    render() {

        let requestors = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderNodeStatsTable(this.state.stats.requestors);
        let providers = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderNodeStatsTable(this.state.stats.providers);


        return (
            <div>
                <h1>Market Statistics</h1>
                <p>Golem Market Mockup control panel.</p>
                <div>
                    <h2>Requestors</h2>
                    {requestors}
                </div>
                <div>
                    <h2>Providers</h2>
                    {providers}
                </div>
            </div>
        );
  }
}
