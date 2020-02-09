﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace TrisvagoHotels.FunctionalTests.Fixtures {
	public class HostFixture {
		public TestServer Server { get; }

		public HostFixture() {
			Server = new TestServer(
				new WebHostBuilder()
				.ConfigureServices(services => {
					services.AddSingleton<ISpeakerService, SpeakersService>();
					services.TryAddSingleton<INotificationQueue, NoNotificationQueue>();
				})
				.UseStartup<TestStartup>()
			);
		}
	}
}
