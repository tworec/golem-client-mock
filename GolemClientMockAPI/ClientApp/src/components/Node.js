import React, { Component } from 'react';
import { Subscriptions } from './Subscriptions';

export class Node extends Component {
  static displayName = Node.name;

  constructor (props) {
    super(props);
      this.state = { node: props.node, details: false };
  }

    toggleNodeDetails(nodeId) {

        if (this.state.details == false) {
            fetch('admin/marketStats/' + nodeId)
                .then(response => response.json())
                .then(data => {
                    console.log("NodeDetails loaded:");
                    console.log(data);

                    // replace state with received object!
                    this.setState({ node: data, details: true });
                });
        }
        else {
            this.setState({ node: this.state.node, details: false });
        }
    }

    //setState(newState) {
    //    console.log("Node.setState() called...");
    //    super.setState(newState);
    //}

    formatDateInterval(dateString, now) {
        var date = Date.parse(dateString);

        var span = Math.round((now - date) / 1000);

        return "" + span + " seconds ago";
    }


    render() {
        if (this.state.node != null) {
            var now = Date.now();

            return (
                <div key={this.state.node.nodeId} >
                    <div className='row'>
                        <div className='col-sm-3'><a href='#' onClick={() => this.toggleNodeDetails(this.state.node.nodeId)} >{this.state.node.nodeId}</a></div>
                        <div className='col-sm-3'>{this.formatDateInterval(this.state.node.connected, now)}</div>
                        <div className='col-sm-2'>{this.state.node.subscriptionCount}</div>
                        <div className='col-sm-3'>{this.formatDateInterval(this.state.node.lastActive, now)}</div>
                    </div>
                    {this.renderSubscriptions()}
                </div>
          );

      }
      else {
          return "";
      }
    }

    renderSubscriptions() {
        if (this.state.details) {
            return (
                <Subscriptions subscriptions={this.state.node.subscriptions} />
            );
        }
        else {
            return "";
        }
    }
}
