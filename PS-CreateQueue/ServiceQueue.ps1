Add-azureaccount

get-azureRMSubscription

Select-AzureSubscription -SubscriptionName ''


get-AzureSBNameSpace

$SBName = 'sqlrelay2016sb'

remove-AzureSBNameSpace -Name $SBName

$SBNameSpace = New-AzureSBNameSpace -Name $SBName -Location 'North Europe' -CreateACSNamespace $True -NamespaceType Messaging 

$SBNameSpace.ConnectionString

#PS Scripts to create queues, topics and Subscriptions are by Paolo Salvatori - http://blogs.msdn.com/b/paolos/

. .\CreateQueue.ps1 -Path SQLRelayMessages -Namespace $SBName -DefaultMessageTimeToLive 60 -EnablePartitioning $True -UserMetadata 'SQLrelay queue' -RequiresSession $True -Location 'North Europe'

. .\createtopic.ps1 -Path relay2016topic  -Namespace $SBName -DefaultMessageTimeToLive 60 -EnablePartitioning $True 


. .\CreateSubscription.ps1 -Namespace $SBName -name relay2016topicOdds -TopicPath relay2016topic -DefaultMessageTimeToLive 60  -RequiresSession $true -SupportOrdering $True -SqlFilter 'FilterID=1' -SqlRuleAction 'SET Priority="Medium"'

. .\CreateSubscription.ps1 -Namespace $SBName -name relay2016topicEvens -TopicPath relay2016topic -DefaultMessageTimeToLive 60  -RequiresSession $true -SupportOrdering $True -SqlFilter 'FilterID=0' -SqlRuleAction 'SET Priority="Medium"'

