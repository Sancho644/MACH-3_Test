mergeInto(LibraryManager.library, {

    SubmitScore: function (score)
    {
        if (typeof ysdk === 'undefined')
            return;

        ysdk.getLeaderboards()
            .then(lb => {
                lb.setLeaderboardScore(
                    'mainLeaderboard',
                    score
                );
            });
    },

    GetLeaderboardEntries: function ()
    {
        if (typeof ysdk === 'undefined')
            return;

        ysdk.getLeaderboards()
            .then(lb => {

                lb.getLeaderboardEntries(
                    'mainLeaderboard',
                    {
                        quantityTop: 10,
                        includeUser: false,
                        quantityAround: 0
                    })
                    .then(result => {

                        const entries = result.entries.map(e => ({
                            name: e.player.publicName || 'Anonymous',
                            score: e.score
                        }));

                        const json = JSON.stringify(entries);

                        SendMessage(
                            'LeaderboardBridge',
                            'OnLeaderboardLoaded',
                            json
                        );
                    });
            });
    }
});