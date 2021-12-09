using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Constructs;
using System;
using System.Collections.Generic;

namespace orders.api
{
    public class AWSStack : Stack
    {
        public AWSStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var vpc = new Amazon.CDK.AWS.EC2.Vpc(this, "VPC", new VpcProps()
            {
                SubnetConfiguration = new ISubnetConfiguration[]
                {
                    new SubnetConfiguration()
                    {
                         Name = "Public",
                         SubnetType = SubnetType.PUBLIC,
                         CidrMask = 24                      
                    }
                }
            });
            var securityGroup = new SecurityGroup(this, "myVmSecGroup", new SecurityGroupProps()
            {
                Vpc = vpc,
                AllowAllOutbound = true,                
            });

            securityGroup.AddIngressRule(Peer.AnyIpv4(), Port.Tcp(80), "httpIpv4");
            securityGroup.AddIngressRule(Peer.AnyIpv6(), Port.Tcp(80), "httpIpv6");

            var instanceType = new InstanceType("t4g.micro");
            // Pick the right Amazon Linux edition. All arguments shown are optional
            // and will default to these values when omitted.
            // For other custom (Linux) images, instantiate a `GenericLinuxImage` with
            // a map giving the AMI to in for each region:
            IMachineImage linux = MachineImage.GenericLinux(new Dictionary<string, string> {
                { "us-east-1", "ami-0a1eddae0b7f0a79f" }
            });

            var instance = new Amazon.CDK.AWS.EC2.Instance_(this, "MyInstace", new InstanceProps()
            {
                Vpc = vpc,
                MachineImage = linux,
                InstanceType = instanceType,
                SecurityGroup = securityGroup,
                VpcSubnets = new SubnetSelection()
                {
                    SubnetType = SubnetType.PUBLIC
                },
                Init = CloudFormationInit.FromElements(
                    InitCommand.ShellCommand("sudo amazon-linux-extras install nginx1")
                )
       
            });

            // OUTPUTS:
            //var webUrl = new CfnOutput(this, "mySiteUrl", new CfnOutputProps()
            //{
            //    Value = $"http://{instance.InstancePublicIp}/",
            //    Description = "My instance url",
            //    ExportName = "orderService"
            //});
            /*var cluster = new Cluster(this, "order_cluster", new ClusterProps() {  Vpc = vpc });
            var taskDefinition = new Ec2TaskDefinition(this, "order_task");
            var capacityOptions = new AddCapacityOptions
            {
                AutoScalingGroupName = "order_auto_scale",
                InstanceType = InstanceType.Of(InstanceClass.STANDARD6_GRAVITON, InstanceSize.MICRO)              
            };
            taskDefinition.AddContainer("DefaultContainer", new ContainerDefinitionOptions
            {
                Image = ContainerImage.FromRegistry("amazon/amazon-ecs-sample"),
                MemoryLimitMiB = 512
            });
            // set default cluster configuration    
            cluster.AddCapacity("order_cap", capacityOptions);
            var ec2Service = new Ec2Service(this, "orders", new Ec2ServiceProps() { Cluster = cluster, TaskDefinition = taskDefinition });
            */
            // Api gateway
            // Lambda Function
            // Dynamodb
            // Eventbridge
            // Kinesis Stream
        }
    }
}

